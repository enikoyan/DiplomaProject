using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly EdSystemDbContext _context;
        private readonly ISquadService _squadService;
        private readonly ICourseService _courseService;
        private readonly IFileManagementService _fileManagementService;

        public MaterialService(EdSystemDbContext context, ISquadService squadService, ICourseService courseService, IFileManagementService fileManagementService)
        {
            _context = context;
            _squadService = squadService;
            _courseService = courseService;
            _fileManagementService = fileManagementService;
        }

        private Guid GenerateGuid() => Guid.NewGuid();

        public async Task<bool> CreateMaterial(List<IFormFile> files, string groupBy, List<string> foreignKeys)
        {
            foreach (var file in files)
            {
                var newId = GenerateGuid();

                switch (groupBy)
                {
                    case "searchByCourses":
                        {
                            List<int> coursesIds = await _courseService.GetCoursesIdsByNames(foreignKeys);

                            // Create fileItem in DB
                            Models.File fileItem = new Models.File()
                            {
                                Id = newId,
                                Title = Path.GetFileNameWithoutExtension(file.FileName),
                                Type = Path.GetExtension(file.FileName),
                                DateAdded = DateTime.UtcNow
                            };

                            await _context.Files.AddAsync(fileItem);

                            foreach (var coursesId in coursesIds)
                            {
                                // Create materialItem row in DB
                                Material materialItem = new Material()
                                {
                                    IdFile = newId,
                                    IdCourse = coursesId,
                                };

                                await _context.Materials.AddAsync(materialItem);
                            }
                            break;
                        }
                    case "searchBySquads":
                        {
                            // Get squads Ids
                            List<int> squadsIds = await _squadService.GetSquadsIdsByNames(foreignKeys);

                            // Get courses Ids
                            List<int> coursesIds = await _squadService.GetCoursesIdsByNames(squadsIds);

                            for (int i = 0; i < foreignKeys.Count; i++)
                            {
                                // Create fileItem in DB
                                Models.File fileItem = new Models.File()
                                {
                                    Id = newId,
                                    Title = Path.GetFileNameWithoutExtension(file.FileName),
                                    Type = Path.GetExtension(file.FileName),
                                    DateAdded = DateTime.UtcNow
                                };

                                await _context.Files.AddAsync(fileItem);

                                // Create materialItem row in DB
                                Material materialItem = new Material()
                                {
                                    IdFile = newId,
                                    IdCourse = coursesIds[i],
                                    IdSquad = squadsIds[i]
                                };

                                await _context.Materials.AddAsync(materialItem);
                            }
                            break;
                        }
                    default: return false;
                }

                await _context.SaveChangesAsync();

                // Upload file to the server
                try
                {
                    string fileName = newId.ToString();
                    var newFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, $"{fileName}{Path.GetExtension(file.FileName)}");
                    await _fileManagementService.UploadFileAsync(newFile, "Materials");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return true;
        }

        public async Task<List<MaterialWithFile>> GetAllMaterials()
        {
            var materials = await _context.Materials
                    .Include(m => m.IdFileNavigation)
                    .ToListAsync();

            if (materials.Count == 0 || materials == null)
            {
                throw new Exception("Такого файла нет!");
            }

            return await AddMaterialsToList(materials);
        }

        public async Task<List<MaterialWithFile>> GetMaterialsByType(string fileType)
        {
            var materials = await _context.Materials
                        .Include(m => m.IdFileNavigation)
                        .Where(m => m.IdFileNavigation.Type == fileType)
                        .ToListAsync();

            if (materials.Count == 0 || materials == null)
            {
                throw new Exception("Такого файла нет!");
            }

            return await AddMaterialsToList(materials);
        }

        public async Task<MaterialWithFile> GetMaterialById(Guid materialId)
        {
            var material = await _context.Materials
                   .Include(m => m.IdFileNavigation)
                   .FirstOrDefaultAsync(m => m.IdFile == materialId);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            return await AddMaterialsToList(material);
        }

        public async Task<MaterialWithFile> GetMaterialByTitle(string materialTitle)
        {
            var material = await _context.Materials
                .Include(m => m.IdFileNavigation)
                .FirstOrDefaultAsync(m => m.IdFileNavigation.Title == materialTitle);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            return await AddMaterialsToList(material);
        }

        public async Task<List<MaterialWithFile>> GetMaterialsBySquad(string squadName)
        {
            // Get squadId by squadName
            var squadId = await _squadService.GetSquadIdByName(squadName);

            List<Material> materials = await _context.Materials
                .Include(m => m.IdFileNavigation)
                .Where(m => m.IdSquad == squadId)
                .ToListAsync();

            var distinctMaterials = materials.GroupBy(m => m.IdFile)
                                                 .Select(g => g.First())
                                                 .ToList();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для группы {squadName} не найдено!");
            }

            return await AddMaterialsToList(distinctMaterials);
        }

        public async Task<List<MaterialWithFile>> GetMaterialsByCourse(string courseName)
        {
            // Get courseId by courseName
            var courseId = await _courseService.GetCourseIdByName(courseName);

            List<Material> materials = await _context.Materials
                .Include(m => m.IdFileNavigation)
                .Where(m => m.IdCourse == courseId)
                .ToListAsync();

            var distinctMaterials = materials.GroupBy(m => m.IdFile)
                                                 .Select(g => g.First())
                                                 .ToList();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для курса {courseName} не найдено!");
            }

            return await AddMaterialsToList(distinctMaterials);
        }

        public async Task<IActionResult> DownloadMaterial(Guid materialId)
        {
            var material = await GetMaterialById(materialId);

            var file = await _context.Files.FirstOrDefaultAsync(s => s.Id == materialId);

            if (file != null)
            {
                return await _fileManagementService.DownloadFileAsync(materialId.ToString(), "Materials", file.Title);
            }
            else throw new Exception("Файл не найден!");
        }

        public async Task<bool> DeleteSquadMaterial(Guid materialId, string squadName)
        {
            // Delete material in the database
            int squadId = await _squadService.GetSquadIdByName(squadName);
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.IdSquad == squadId && m.IdFile == materialId);

            if (material != null)
            {
                _context.Materials.Remove(material);

                await _context.SaveChangesAsync();

                // Check If some squad has this file
                material = await _context.Materials.FirstOrDefaultAsync(m => m.IdFile == materialId);

                if (material == null)
                {
                    // Delete file from server
                    await _fileManagementService.DeleteFileAsync(materialId.ToString(), "Materials");

                    // Delete file in the database
                    _context.Files.Remove(_context.Files.FirstOrDefault(s => s.Id == materialId)!);
                    await _context.SaveChangesAsync();
                }

                return true;
            }

            else
            {
                throw new Exception($"Материал не найден!");
            }
        }

        public async Task<bool> DeleteCourseMaterial(Guid materialId, string courseName)
        {
            // Delete material in the database
            int courseId = await _courseService.GetCourseIdByName(courseName);
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.IdCourse == courseId && m.IdFile == materialId);

            if (material != null)
            {
                _context.Materials.Remove(material);

                await _context.SaveChangesAsync();

                // Check If some squad has this file
                material = await _context.Materials.FirstOrDefaultAsync(m => m.IdFile == materialId);

                if (material == null)
                {
                    // Delete file from server
                    await _fileManagementService.DeleteFileAsync(materialId.ToString(), "Materials");

                    // Delete file in the database
                    _context.Files.Remove(_context.Files.FirstOrDefault(s => s.Id == materialId)!);
                    await _context.SaveChangesAsync();
                }

                return true;
            }

            else
            {
                throw new Exception($"Материал не найден!");
            }
        }

        #region Private methods
        private async Task<List<MaterialWithFile>> AddMaterialsToList(List<Material> materials)
        {
            List<MaterialWithFile> result = new List<MaterialWithFile>();

            foreach (var material in materials)
            {
                var materialWithFile = new MaterialWithFile
                {
                    Id = material.Id,
                    IdFile = material.IdFile,
                    IdCourse = material.IdCourse,
                    IdSquad = material.IdSquad,
                    File = material.IdFileNavigation
                };

                result.Add(materialWithFile);
            }

            return result;
        }

        private async Task<MaterialWithFile> AddMaterialsToList(Material material)
        {
            var materialWithFile = new MaterialWithFile
            {
                Id = material.Id,
                IdFile = material.IdFile,
                IdCourse = material.IdCourse,
                IdSquad = material.IdSquad,
                File = material.IdFileNavigation
            };

            return materialWithFile;
        }
        #endregion
    }
}

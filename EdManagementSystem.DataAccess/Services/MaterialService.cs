﻿using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly User004Context _context;
        private readonly ISquadService _squadService;
        private readonly ICourseService _courseService;
        private readonly IFileLoadService _fileLoadService;

        public MaterialService(User004Context context, ISquadService squadService, ICourseService courseService, IFileLoadService fileLoadService)
        {
            _context = context;
            _squadService = squadService;
            _courseService = courseService;
            _fileLoadService = fileLoadService;
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

                            foreach (var coursesId in coursesIds)
                            {
                                // Create materialItem row in DB
                                Material materialItem = new Material();
                                materialItem.MaterialId = newId;
                                materialItem.Title = Path.GetFileNameWithoutExtension(file.FileName);
                                materialItem.IdCourse = coursesId;
                                materialItem.Type = Path.GetExtension(file.FileName);
                                materialItem.DateAdded = DateTime.UtcNow;
                                _context.Materials.Add(materialItem);
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
                                // Create materialItem row in DB
                                Material materialItem = new Material();
                                materialItem.MaterialId = newId;
                                materialItem.Title = Path.GetFileNameWithoutExtension(file.FileName);
                                materialItem.IdCourse = coursesIds[i];
                                materialItem.IdSquad = squadsIds[i];
                                materialItem.Type = Path.GetExtension(file.FileName);
                                materialItem.DateAdded = DateTime.UtcNow;
                                _context.Materials.Add(materialItem);
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
                    var newFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, $"{fileName}.{Path.GetExtension(file.FileName)}");
                    await _fileLoadService.UploadFileAsync(newFile);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return true;
        }

        public async Task<List<Material>> GetAllMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material> GetMaterialById(Guid materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.MaterialId == materialId);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            else return material;
        }

        public async Task<List<Material>> GetMaterialsByType(string fileType)
        {
            return await _context.Materials.Where(u => u.Type == fileType).ToListAsync();
        }

        public async Task<Material> GetMaterialByTitle(string materialTitle)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Title == materialTitle);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            else return material;
        }

        public async Task<List<Material>> GetMaterialsBySquad(string squadName)
        {
            // Get squadId by squadName
            var squadId = await _squadService.GetSquadIdByName(squadName);

            List<Material> materials = await _context.Materials.Where(m => m.IdSquad == squadId).ToListAsync();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для группы {squadName} не найдено!");
            }

            return materials;
        }

        public async Task<List<Material>> GetMaterialsByCourse(string courseName)
        {
            // Get courseId by courseName
            var courseId = await _courseService.GetCourseIdByName(courseName);

            List<Material> materials = await _context.Materials.Where(m => m.IdCourse == courseId).ToListAsync();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для курса {courseName} не найдено!");
            }

            return materials;
        }
    }
}

using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.DTO;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly EdSystemDbContext _dbContext;
        private readonly ICourseService _courseService;
        private readonly ISquadService _squadService;
        private readonly IFileManagementService _fileManagementService;

        private const string folderName = "Homeworks";

        public HomeworkService(EdSystemDbContext dbContext, ICourseService courseService, ISquadService squadService, IFileManagementService fileManagementService)
        {
            _dbContext = dbContext;
            _courseService = courseService;
            _squadService = squadService;
            _fileManagementService = fileManagementService;
        }

        public async Task<bool> CreateHomework(string groupBy, List<string> foreignKeys,
            string title, string? description, string? note, DateTime? deadline, List<IFormFile>? files)
        {
            // Attributes for adding new entity
            DateTime addedTime = DateTime.UtcNow;
            var newId = Guid.NewGuid();

            switch (groupBy)
            {
                case "searchByCourses":
                    {
                        // Get courses Ids
                        List<int> coursesIds = await _courseService.GetCoursesIdsByNames(foreignKeys);

                        // Check collision
                        for (int i = 0; i < coursesIds.Count; i++)
                        {
                            if (await CheckTitleCollisionByCourse(coursesIds[i], title))
                            {
                                var course = await _courseService.GetCourseById(coursesIds[i]);

                                throw new Exception($"Смените название Д/З!" +
                                    $" Домашнее задание с таким названием уже существует для курса {course.CourseName}");
                            }
                        }

                        // Create homeworks in database
                        foreach (var item in coursesIds)
                        {
                            Homework homework = new Homework()
                            {
                                HomeworkId = newId,
                                CourseId = item,
                                DateAdded = addedTime,
                                Deadline = deadline,
                                Title = title,
                                Description = description,
                                Note = note,
                            };

                            await _dbContext.Homeworks.AddAsync(homework);
                            await _dbContext.SaveChangesAsync();
                        }

                        // Upload attached files
                        if (files?.Count > 0 || files != null)
                        {
                            await AddHomeworkFile(newId, files);
                        }

                        return true;
                    }
                case "searchBySquads":
                    {
                        // Get squads Ids
                        List<int> squadsIds = await _squadService.GetSquadsIdsByNames(foreignKeys);

                        // Get courses Ids
                        List<int> coursesIds = await _squadService.GetCoursesIdsByNames(squadsIds);

                        // Check collision
                        for (int i = 0; i < squadsIds.Count; i++)
                        {
                            if (await CheckTitleCollisionBySquad(squadsIds[i], coursesIds[i], title))
                            {
                                var squad = await _squadService.GetSquadById(squadsIds[i]);
                                var course = await _courseService.GetCourseById(coursesIds[i]);

                                throw new Exception($"Смените название Д/З!" +
                                    $" Домашнее задание с таким названием уже существует для курса {course.CourseName} " +
                                    $"и группы {squad.SquadName}");
                            }
                        }

                        // Create homeworks in database
                        for (int i = 0; i < squadsIds.Count; i++)
                        {
                            Homework homework = new Homework()
                            {
                                HomeworkId = newId,
                                SquadId = squadsIds[i],
                                CourseId = coursesIds[i],
                                DateAdded = addedTime,
                                Deadline = deadline,
                                Title = title,
                                Description = description,
                                Note = note,
                            };

                            await _dbContext.Homeworks.AddAsync(homework);
                            await _dbContext.SaveChangesAsync();
                        }

                        // Upload attached files
                        if (files?.Count > 0 || files != null) { await AddHomeworkFile(newId, files); }

                        return true;
                    }
                default: { return false; }
            }
        }

        public async Task<List<HomeworkDTO>> GetAllHomeworks()
        {
            var homeworks = await _dbContext.Homeworks.ToListAsync();
            return await CreateHomeworkDTO(homeworks);
        }

        public async Task<List<HomeworkDTO>> GetHomeworksByCourse(string courseName)
        {
            // Get course by name
            var course = await _courseService.GetCourseByName(courseName);

            // Get homeworks of current course
            var homeworks = await _dbContext.Homeworks
                .Where(s => s.CourseId == course.CourseId)
                .OrderBy(s => s.DateAdded)
                .ThenBy(s => s.Deadline)
                .ToListAsync();

            var result = await CreateHomeworkDTO(homeworks);

            if (result != null)
            {
                return result;
            }

            else throw new Exception($"Домашние задания для курса {course.CourseName} не найдены!");
        }

        public async Task<List<HomeworkDTO>> GetHomeworksBySquad(string squadName)
        {
            // Get squad by name
            var squad = await _squadService.GetSquadByName(squadName);

            // Get homeworks of current squad
            var homeworks = await _dbContext.Homeworks
                .Where(s => s.SquadId == squad.SquadId)
                .OrderBy(s => s.DateAdded)
                .ThenBy(s => s.Deadline)
                .ToListAsync();

            var result = await CreateHomeworkDTO(homeworks);

            if (result != null)
            {
                return result;
            }

            else throw new Exception($"Домашние задания для группы {squad.SquadName} не найдены!");
        }

        public async Task<List<HomeworkDTO>> GetHomeworksByTitle(string title)
        {
            var homeworks = await _dbContext.Homeworks.Where(s => s.Title == title).ToListAsync();

            return await CreateHomeworkDTO(homeworks);
        }

        public async Task<HomeworkDTO> GetHomeworkById(Guid homeworkId)
        {
            return new HomeworkDTO
            {
                Homework = await _dbContext.Homeworks.FirstOrDefaultAsync(s => s.HomeworkId == homeworkId),
                AttachedFiles = await GetAttachedFiles(homeworkId),
            };
        }

        public async Task<bool> DeleteHomework(Guid homeworkId, string groupBy, List<string> foreignKeys)
        {
            switch (groupBy)
            {
                case "searchByCourses":
                    {
                        // Get courses Ids
                        List<int> coursesIds = await _courseService.GetCoursesIdsByNames(foreignKeys);

                        // Get the list of homeworks with the given id and for given courses
                        List<Homework> homeworksList = await _dbContext.Homeworks
                            .Where(s => s.HomeworkId == homeworkId && coursesIds.Contains(s.CourseId))
                            .ToListAsync();

                        if (homeworksList != null)
                        {
                            return await RemoveHomeworkEntity(homeworkId, homeworksList);
                        }
                        else throw new Exception("Домашние задания с данным идентификатором не найдены!");
                    }
                case "searchBySquads":
                    {
                        // Get squads Ids
                        List<int> squadsIds = await _squadService.GetSquadsIdsByNames(foreignKeys);

                        // Get the list of homeworks with the given id and for given courses
                        var homeworksList = await _dbContext.Homeworks
                            .Where(s => s.HomeworkId == homeworkId && squadsIds.Contains((int)s.SquadId!))
                            .ToListAsync();

                        if (homeworksList != null)
                        {
                            return await RemoveHomeworkEntity(homeworkId, homeworksList);
                        }
                        else throw new Exception("Домашние задания с данным идентификатором не найдены!");
                    }
                default: return false;
            }
        }

        public async Task<bool> DeleteAttachedFile(Guid homeworkId, Guid fileId)
        {
            try
            {
                var attachedFile = await _dbContext.HomeworkFiles
                    .FirstOrDefaultAsync(s => s.HomeworkId == homeworkId && s.FileId == fileId);

                if (attachedFile != null)
                {
                    _dbContext.HomeworkFiles.Remove(attachedFile);
                    await _dbContext.SaveChangesAsync();

                    // If there is no more attachedFiles then delete file from server and database
                    if (!await VerifyAttachedFileExisting(fileId))
                    {
                        _dbContext.Files.Remove(await _dbContext.Files.FirstOrDefaultAsync(s => s.Id == fileId));
                        await _dbContext.SaveChangesAsync();
                    }

                    await _fileManagementService.DeleteFileAsync(fileId.ToString(), folderName);

                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddHomeworkFile(Guid homeworkId, List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    Guid newId = Guid.NewGuid();

                    // Add file row in database
                    Models.File fileItem = new Models.File()
                    {
                        Id = newId,
                        Title = Path.GetFileNameWithoutExtension(file.FileName),
                        Type = Path.GetExtension(file.FileName),
                        DateAdded = DateTime.UtcNow
                    };

                    await _dbContext.Files.AddAsync(fileItem);

                    // Add relationship from File to Homework
                    HomeworkFile homeworkFile = new HomeworkFile()
                    {
                        FileId = newId,
                        HomeworkId = homeworkId
                    };

                    await _dbContext.HomeworkFiles.AddAsync(homeworkFile);

                    await _dbContext.SaveChangesAsync();

                    // Upload file to the server
                    string fileName = newId.ToString();
                    var newFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, $"{fileName}{Path.GetExtension(file.FileName)}");
                    await _fileManagementService.UploadFileAsync(newFile, "Homeworks");
                }

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
        }

        public async Task<List<Models.File>> GetAttachedFiles(Guid homeworkId)
        {
            try
            {
                var attachedFilesList = await _dbContext.HomeworkFiles
                    .Where(f => f.HomeworkId == homeworkId)
                    .Select(s => s.FileId)
                    .ToListAsync();

                return await _dbContext.Files.Where(s => attachedFilesList.Contains(s.Id)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IActionResult> DownloadHomeworks(Guid homeworkId)
        {
            var attachedFiles = await _dbContext.HomeworkFiles
                                .Where(f => f.HomeworkId == homeworkId)
                                .Select(s => s.FileId.ToString())
                                .ToListAsync();

            var homework = await _dbContext.Homeworks
                .FirstOrDefaultAsync(s => s.HomeworkId == homeworkId);

            string archiveName = $"{homework!.Title}. {homework.DateAdded.ToShortDateString()}";

            List<string> attachedFileNames = new List<string>();

            if (attachedFiles == null || attachedFiles.Count == 0)
            {
                throw new Exception("Файлы не найдены!");
            }

            else if (attachedFiles.Count > 1)
            {
                for (int i = 0; i < attachedFiles.Count; i++)
                {
                    var attachedFileTitle = await _dbContext.Files.FirstOrDefaultAsync(s => s.Id == Guid.Parse(attachedFiles[i]));

                    attachedFileNames.Add(attachedFileTitle!.Title);
                }

                return await _fileManagementService.DownloadFilesAsync(attachedFiles, folderName, attachedFileNames, archiveName);
            }

            else
            {
                var attachedFileName = await _dbContext.Files.FirstOrDefaultAsync(s => s.Id == Guid.Parse(attachedFiles[0]));

                return await _fileManagementService.DownloadFileAsync(attachedFiles[0], folderName, attachedFileName!.Title);
            }
        }

        public async Task<bool> UpdateHomework(Guid homeworkId, string attributeName, string value)
        {
            try
            {
                var homeworks = await _dbContext.Homeworks.Where(s => s.HomeworkId == homeworkId).ToListAsync();

                if (homeworks != null || homeworks!.Count > 0)
                {
                    foreach (var homework in homeworks)
                    {
                        typeof(Homework).GetProperty(attributeName)!.SetValue(homework, value);
                        _dbContext.Homeworks.Update(homework);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangeHomeworkDeadline(Guid homeworkId, DateTime deadline)
        {
            try
            {
                var homeworks = await _dbContext.Homeworks.Where(s => s.HomeworkId == homeworkId).ToListAsync();

                if (homeworks != null || homeworks!.Count > 0)
                {
                    foreach (var homework in homeworks)
                    {
                        homework.Deadline = deadline;
                        _dbContext.Homeworks.Update(homework);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Private functions
        // Create homework DTO (with attached files)
        private async Task<List<HomeworkDTO>> CreateHomeworkDTO(List<Homework> homeworks)
        {
            List<HomeworkDTO> result = new List<HomeworkDTO>();

            foreach (var homework in homeworks)
            {
                result.Add(new HomeworkDTO
                {
                    Homework = homework,
                    AttachedFiles = await GetAttachedFiles(homework.HomeworkId),
                });
            }

            return result;
        }

        // Remove homework entity from database
        private async Task<bool> RemoveHomeworkEntity(Guid homeworkId, List<Homework> homeworksList)
        {
            try
            {
                // Remove all found homeworks
                if (!await RemoveHomeworksList(homeworksList))
                {
                    return false;
                }

                // Check is there still some homeworks with the given id and then delete attached files if it's false
                if (!await VerifyHomeworksExisting(homeworkId))
                {
                    return await RemoveAttachedFile(homeworkId);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Remove the given list of homeworks
        private async Task<bool> RemoveHomeworksList(List<Homework> homeworksList)
        {
            try
            {
                _dbContext.Homeworks.RemoveRange(homeworksList);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Verify is there more homeworks with the given id
        private async Task<bool> VerifyHomeworksExisting(Guid homeworkId)
        {
            var checkList = await _dbContext.Homeworks.Where(s => s.HomeworkId == homeworkId).ToListAsync();
            if (checkList.Count > 0)
            {
                return true;
            }
            else return false;
        }

        // Remove all the attached files of the given homework
        private async Task<bool> RemoveAttachedFile(Guid homeworkId)
        {
            try
            {
                List<Guid> fileList = new List<Guid>();

                // Get the list of attached files with the given homeworkId
                var attachedFiles = await _dbContext.HomeworkFiles.Where(s => s.HomeworkId == homeworkId).ToListAsync();

                foreach (var file in attachedFiles)
                {
                    fileList.Add(file.FileId);
                    _dbContext.HomeworkFiles.Remove(file);
                }

                await _dbContext.SaveChangesAsync();

                return await RemoveFilesList(fileList);
            }
            catch
            {
                return false;
            }
        }

        // Remove the given list of files
        private async Task<bool> RemoveFilesList(List<Guid> fileList)
        {
            try
            {
                // Remove from File table
                var files = await _dbContext.Files.Where(s => fileList.Contains(s.Id)).ToListAsync();
                var deleteFiles = files.Select(s => s.Id.ToString()).ToList();
                _dbContext.Files.RemoveRange(files);
                await _dbContext.SaveChangesAsync();

                // Delete from server
                await _fileManagementService.DeleteFileAsync(deleteFiles, folderName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Verify is there more attached files with the given id
        private async Task<bool> VerifyAttachedFileExisting(Guid fileId)
        {
            var attachedFile = await _dbContext.HomeworkFiles.FirstOrDefaultAsync(s => s.FileId == fileId);

            if (attachedFile != null)
            {
                return true;
            }
            else return false;
        }

        // Check is there homework with the same title for the same course
        private async Task<bool> CheckTitleCollisionByCourse(int courseId, string title)
        {
            var item = await _dbContext.Homeworks.FirstOrDefaultAsync(s => s.Title == title && s.CourseId == courseId);

            if (item == null) { return false; }
            else return true;
        }

        // Check is there homework with the same title for the same squad
        private async Task<bool> CheckTitleCollisionBySquad(int squadId, int courseId, string title)
        {
            var item = await _dbContext.Homeworks
                .FirstOrDefaultAsync(s => s.Title == title && s.SquadId == squadId && s.CourseId == courseId);

            if (item == null) { return false; }
            else return true;
        }
        #endregion
    }
}
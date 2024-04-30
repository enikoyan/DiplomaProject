using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly EdSystemDbContext _dbContext;
        private readonly ICourseService _courseService;
        private readonly ISquadService _squadService;

        public HomeworkService(EdSystemDbContext dbContext, ICourseService courseService, ISquadService squadService)
        {
            _dbContext = dbContext;
            _courseService = courseService;
            _squadService = squadService;
        }

        public async Task<bool> CreateHomework(string groupBy, List<string> foreignKeys,
            string title, string? description, string? note, DateTime? deadline)
        {
            // Attributes for adding new entity
            DateTime addedTime = DateTime.UtcNow;

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
                                HomeworkId = Guid.NewGuid(),
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
                                HomeworkId = Guid.NewGuid(),
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
                        return true;
                    }
                default: { return false; }
            }
        }

        public async Task<List<Homework>> GetAllHomeworks() => await _dbContext.Homeworks.ToListAsync();

        public async Task<List<Homework>> GetHomeworksByCourse(string courseName)
        {
            // Get course by name
            var course = await _courseService.GetCourseByName(courseName);

            // Get homeworks of current course
            var result = await _dbContext.Homeworks
                .Where(s => s.CourseId == course.CourseId)
                .OrderBy(s => s.DateAdded)
                .ThenBy(s => s.Deadline)
                .ToListAsync();

            if (result != null)
            {
                return result;
            }

            else throw new Exception($"Домашние задания для курса {course.CourseName} не найдены!");
        }

        public async Task<List<Homework>> GetHomeworksBySquad(string squadName)
        {
            // Get squad by name
            var squad = await _squadService.GetSquadByName(squadName);

            // Get homeworks of current squad
            var result = await _dbContext.Homeworks
                .Where(s => s.SquadId == squad.SquadId)
                .OrderBy(s => s.DateAdded)
                .ThenBy(s => s.Deadline)
                .ToListAsync();

            if (result != null)
            {
                return result;
            }

            else throw new Exception($"Домашние задания для группы {squad.SquadName} не найдены!");
        }

        public async Task<List<Homework>> GetHomeworksByTitle(string title)
            => await _dbContext.Homeworks.Where(s => s.Title == title).ToListAsync();

        public async Task<Homework> GetHomeworkById(Guid homeworkId)
    => await _dbContext.Homeworks.FirstOrDefaultAsync(s => s.HomeworkId == homeworkId);

        public async Task<bool> DeleteHomework(Guid homeworkId)
        {
            try
            {
                var item = await _dbContext.Homeworks.FirstOrDefaultAsync(s => s.HomeworkId == homeworkId);

                if (item != null)
                {
                    _dbContext.Homeworks.Remove(item!);

                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                else {
                    throw new Exception("Домашнее задание с данным идентификатором не найдено!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #region Private functions
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

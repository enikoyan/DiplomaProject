using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly EdSystemDbContext _dbContext;
        private readonly ITeacherService _teacherService;
        private readonly ISquadService _squadService;

        public ScheduleService(EdSystemDbContext dbContext, ITeacherService teacherService, ISquadService squadService)
        {
            _dbContext = dbContext;
            _teacherService = teacherService;
            _squadService = squadService;
        }

        public async Task<List<Schedule>> GetAllScheduleItems()
        {
            var result = await _dbContext.Schedules
                .OrderBy(s => s.TimelineStart)
                .Select(s => new Schedule
                {
                    Id = s.Id,
                    TeacherId = s.TeacherId,
                    SquadId = s.SquadId,
                    Weekday = s.Weekday,
                    Date = s.Date,
                    TimelineStart = s.TimelineStart,
                    TimelineEnd = s.TimelineEnd,
                    Place = s.Place,
                    Note = s.Note,
                    SquadName = s.Squad.SquadName
                })
                .ToListAsync();

            if (result.Count > 0)
            {
                return result;
            }

            throw new Exception("Расписание не найдено!");
        }

        public async Task<List<Schedule>> GetScheduleByWeek(string teacherEmail, DateOnly weekStart, DateOnly weekEnd)
        {
            var teacher = await _teacherService.GetTeacherByEmail(teacherEmail);

            var result = await _dbContext.Schedules
                .Where(s => (s.Date >= weekStart && s.Date <= weekEnd) && s.TeacherId == teacher.TeacherId)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.TimelineStart)
                .Select(s => new Schedule
                {
                    Id = s.Id,
                    TeacherId = s.TeacherId,
                    SquadId = s.SquadId,
                    Weekday = s.Weekday,
                    Date = s.Date,
                    TimelineStart = s.TimelineStart,
                    TimelineEnd = s.TimelineEnd,
                    Place = s.Place,
                    Note = s.Note,
                    SquadName = s.Squad.SquadName
                })
                .ToListAsync();

            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                throw new Exception($"Расписание в промежутке от {weekStart} по {weekEnd} для преподавателя {teacher.Fio} не найдено!");
            }
        }

        public async Task<List<Schedule>> GetScheduleBySquad(string squadName)
        {
            var squad = await _squadService.GetSquadByName(squadName);

            var result = await _dbContext.Schedules
                .Where(s => s.SquadId == squad.SquadId)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.TimelineStart)
                .Select(s => new Schedule
                {
                    Id = s.Id,
                    TeacherId = s.TeacherId,
                    SquadId = s.SquadId,
                    Weekday = s.Weekday,
                    Date = s.Date,
                    TimelineStart = s.TimelineStart,
                    TimelineEnd = s.TimelineEnd,
                    Place = s.Place,
                    Note = s.Note,
                    SquadName = s.Squad.SquadName
                })
                .ToListAsync();

            if (result.Count > 0)
            {
                return result;
            }

            else throw new Exception($"Для группы {squad.SquadName} расписание не найдено!");
        }

        public async Task<List<Schedule>> GetScheduleByDay(DateOnly date)
        {
            var result = await _dbContext.Schedules
                .Where(s => s.Date == date)
                .Select(s => new Schedule
                {
                    Id = s.Id,
                    TeacherId = s.TeacherId,
                    SquadId = s.SquadId,
                    Weekday = s.Weekday,
                    Date = s.Date,
                    TimelineStart = s.TimelineStart,
                    TimelineEnd = s.TimelineEnd,
                    Place = s.Place,
                    Note = s.Note,
                    SquadName = s.Squad.SquadName
                })
                .ToListAsync();

            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                throw new Exception($"На день {date} расписание отсутствует!");
            }
        }

        public async Task<bool> CreateSchedule(List<Schedule> schedules)
        {
            try
            {
                foreach (var item in schedules)
                {
                    await _dbContext.Schedules.AddAsync(item);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }

            catch
            {
                return false;
            }
        }

    }
}

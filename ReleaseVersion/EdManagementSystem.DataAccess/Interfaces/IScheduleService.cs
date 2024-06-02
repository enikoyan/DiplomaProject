using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IScheduleService
    {
        Task<bool> CreateSchedule(List<Schedule> schedules);
        Task<List<Schedule>> GetAllScheduleItems();
        Task<List<Schedule>> GetScheduleByDay(DateOnly date);
        Task<List<Schedule>> GetScheduleBySquad(string squadName);
        Task<List<Schedule>> GetScheduleByWeek(string teacherEmail, DateOnly weekStart, DateOnly weekEnd);
    }
}
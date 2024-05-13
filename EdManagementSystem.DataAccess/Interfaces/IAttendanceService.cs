using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IAttendanceService
    {
        Task<bool> CreateAttendanceItem(AttendanceDTO attendance, IFormFile attendanceFile);
        Task<List<Attendance>> GetAttendanceList();
        Task<bool> RefreshAttendanceFile(Guid attendanceId, IFormFile file, bool overwrite = true);
    }
}
using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly EdSystemDbContext _dbContext;
        private readonly IFileManagementService _fileManagementService;

        public AttendanceService(EdSystemDbContext dbContext, IFileManagementService fileManagementService)
        {
            _dbContext = dbContext;
            _fileManagementService = fileManagementService;
        }

        public async Task<List<Attendance>> GetAttendanceList() => await _dbContext.Attendances.ToListAsync();

        public async Task<bool> CreateAttendanceItem(AttendanceDTO attendanceDTO, IFormFile attendanceFile)
        {
            try
            {
                // Generate id
                var attendanceId = Guid.NewGuid();
                var attendanceFileId = Guid.NewGuid();

                Attendance attendance = new Attendance
                {
                    Id = attendanceId,
                    AddedDate = DateTime.UtcNow,
                    WeekDate = attendanceDTO.WeekDate,
                    SquadId = attendanceDTO.SquadId,
                    FileId = attendanceFileId,
                };


                // Create attendance row in the database
                await _dbContext.Attendances.AddAsync(attendance);

                // Create file row in the database
                await AddAttendancFile(attendance.FileId, attendanceFile);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> RefreshAttendanceFile(Guid attendanceId, IFormFile file, bool overwrite = true)
        {
            try
            {
                Guid attendanceFileId = await _dbContext.Attendances
                    .Where(s => s.Id == attendanceId)
                    .Select(s => s.FileId)
                    .FirstOrDefaultAsync();

                // Replace file in the server
                string fileName = attendanceFileId.ToString();
                var newFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, $"{fileName}{Path.GetExtension(file.FileName)}");
                await _fileManagementService.UploadFileAsync(newFile, "Attendance", overwrite);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Private methods
        private async Task<bool> AddAttendancFile(Guid attendanceFileId, IFormFile file)
        {
            try
            {
                // Add file row in database
                Models.File fileItem = new Models.File()
                {
                    Id = attendanceFileId,
                    Title = Path.GetFileNameWithoutExtension(file.FileName),
                    Type = Path.GetExtension(file.FileName),
                    DateAdded = DateTime.UtcNow
                };

                await _dbContext.Files.AddAsync(fileItem);
                await _dbContext.SaveChangesAsync();

                // Upload file to the server
                string fileName = attendanceFileId.ToString();
                var newFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, $"{fileName}{Path.GetExtension(file.FileName)}");
                await _fileManagementService.UploadFileAsync(newFile, "Attendance");

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
        }
        #endregion
    }
}

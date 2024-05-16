using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EdManagementSystem.DataAccess.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly EdSystemDbContext _dbContext;
        private readonly IFileManagementService _fileManagementService;
        private readonly ISquadService _squadService;
        private const string folderName = "Attendance";

        public AttendanceService(EdSystemDbContext dbContext, IFileManagementService fileManagementService, ISquadService squadService)
        {
            _dbContext = dbContext;
            _fileManagementService = fileManagementService;
            _squadService = squadService;
        }

        public async Task<List<Attendance>> GetAttendanceList() => await _dbContext.Attendances.ToListAsync();

        public async Task<bool> CreateAttendanceItem(AttendanceDTO attendanceDTO, IFormFile attendanceFile)
        {
            try
            {
                // Get squad by squadName
                var squadId = await _squadService.GetSquadIdByName(attendanceDTO.SquadName);

                var attendanceItem = await _dbContext.Attendances.Where(s => s.SquadId == squadId && s.WeekDate == attendanceDTO.WeekDate).FirstOrDefaultAsync();

                if (attendanceItem == null)
                {
                    // Generate id
                    var attendanceId = Guid.NewGuid();
                    var attendanceFileId = Guid.NewGuid();

                    Attendance attendance = new Attendance
                    {
                        Id = attendanceId,
                        AddedDate = DateTime.UtcNow,
                        WeekDate = attendanceDTO.WeekDate,
                        SquadId = squadId,
                        FileId = attendanceFileId,
                    };


                    // Create attendance row in the database
                    await _dbContext.Attendances.AddAsync(attendance);

                    // Create file row in the database
                    await AddAttendancFile(attendance.FileId, attendanceFile);

                    return true;
                }

                // Overwrite
                else
                {
                    return await RefreshAttendanceFile(attendanceItem.Id, attendanceFile);
                }
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

        public async Task<List<List<string>>> CreateAttendanceMatrix(AttendanceDTO attendanceDTO)
        {
            // Get squad by squadName
            var squadId = await _squadService.GetSquadIdByName(attendanceDTO.SquadName);
            var attendanceItem = await _dbContext.Attendances.Where(s => s.SquadId == squadId && s.WeekDate == attendanceDTO.WeekDate).FirstOrDefaultAsync();

            if (attendanceItem != null)
            {
                var attendanceFile = await _fileManagementService.DownloadFileFromDB(attendanceItem.FileId, folderName);

                using (var package = new ExcelPackage(attendanceFile.FileStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int startRow = 3;
                    int endRow = worksheet.Dimension.End.Row;

                    List<List<string>> matrix = new List<List<string>>();

                    for (int row = startRow; row <= endRow; row++)
                    {
                        List<string> rowData = new List<string>();
                        for (int col = 3; col <= 9; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
                            string status = "";
                            switch (cellValue)
                            {
                                case "+":
                                    status = "attendance-true";
                                    break;
                                case "н":
                                    status = "attendance-false";
                                    break;
                                case "б":
                                    status = "attendance-ill";
                                    break;
                                default: status = "attendance-empty";
                                    break;
                            }

                            rowData.Add(status);
                        }
                        matrix.Add(rowData);
                    }

                    return matrix;
                }
            }

            return null!;
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

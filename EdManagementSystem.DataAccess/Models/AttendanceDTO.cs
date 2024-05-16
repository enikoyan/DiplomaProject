namespace EdManagementSystem.DataAccess.Models
{
    public class AttendanceDTO
    {
        public required string SquadName { get; set; }

        public string WeekDate { get; set; } = null!;
    }
}

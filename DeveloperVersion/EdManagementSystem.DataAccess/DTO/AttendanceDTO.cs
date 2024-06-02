namespace EdManagementSystem.DataAccess.DTO
{
    public class AttendanceDTO
    {
        public required string SquadName { get; set; }

        public string WeekDate { get; set; } = null!;
    }
}

namespace EdManagementSystem.DataAccess.DTO
{
    public class EmailConfigDTO
    {
        public required string SmtpServer { get; set; }
        public required int SmtpPort { get; set; }
        public required string SmtpUsername { get; set; }
        public required string SmtpPassword { get; set; }
    }
}

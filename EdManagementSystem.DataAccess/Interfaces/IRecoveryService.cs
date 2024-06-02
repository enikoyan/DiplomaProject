using EdManagementSystem.DataAccess.DTO;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IRecoveryService
    {
        Task<string> RecoveryProcess(Guid serverKey, Guid clientKey, string newPassword);
        Task<string> RecoveryTrigger(string userEmail, EmailConfigDTO emailConfig);
    }
}
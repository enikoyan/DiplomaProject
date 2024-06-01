using EdManagementSystem.DataAccess.DTO;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IRecoveryService
    {
        Task<bool> RecoveryProcess(Guid serverKey, Guid clientKey, string newPassword);
        Task<Guid> RecoveryTrigger(string userEmail, EmailConfigDTO emailConfig);
    }
}
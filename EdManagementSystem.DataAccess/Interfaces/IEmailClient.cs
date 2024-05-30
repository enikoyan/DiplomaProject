using EdManagementSystem.DataAccess.DTO;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IEmailClient
    {
        bool SendMessage(EmailConfigDTO emailConfig, string targetAddress, string message);
    }
}

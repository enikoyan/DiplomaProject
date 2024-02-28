using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ITechSupportService
    {
        Task<bool> ChangeRequestStatus(int requestId, string newStatus);
        Task<bool> CreateRequest(string userEmail, string requestDescription);
        Task<List<TechSupport>> GetAllRequests();
        Task<TechSupport> GetRequestById(int requestId);
        Task<List<TechSupport>> GetRequestsByStatus(string requestStatus);
        Task<List<TechSupport>> GetUserRequests(string userEmail);
    }
}
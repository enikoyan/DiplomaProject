using EdManagementSystem.DataAccess.Models;
using EdManagementSystem.DataAccess.Services;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ISquadService
    {
        Task<Squad> CreateSquad(Squad squad);
        Task DeleteSquad(int squadId);
        Task DeleteSquad(string squadName);
        Task<List<Squad>> GetAllSquads();
        Task<List<int>> GetCoursesIdsByNames(List<int> squadIds);
        Task<Squad> GetSquadById(int squadId);
        Task<Squad> GetSquadByName(string squadName);
        Task<int> GetSquadIdByName(string squadName);
        Task<List<Squad>> GetSquadsByCourse(int courseId);
        Task<List<int>> GetSquadsIdsByCourse(int courseId);
        Task<List<int>> GetSquadsIdsByNames(List<string> squadNames);
    }
}
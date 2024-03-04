using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ISquadService
    {
        Task<Squad> CreateSquad(Squad squad);
        Task DeleteSquad(int squadId);
        Task DeleteSquad(string squadName);
        Task<List<Squad>> GetAllSquads();
        Task<Squad> GetSquadById(int squadId);
        Task<Squad> GetSquadByName(string squadName);
        Task<List<Squad>> GetSquadsByCourse(int courseId);
        Task<List<int>> GetSquadsIdsByCourse(int courseId);
    }
}
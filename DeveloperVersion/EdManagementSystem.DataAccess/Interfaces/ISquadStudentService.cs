
namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ISquadStudentService
    {
        Task<bool> CreateSquadStudent(int studentId, int squadId);
        Task<List<int>> GetStudentsIdsBySquad(int squadId);
        Task<List<int>> GetStudentsIdsBySquads(List<int> squadsIds);
    }
}
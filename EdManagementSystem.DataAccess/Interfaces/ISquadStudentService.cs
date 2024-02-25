namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ISquadStudentService
    {
        Task<bool> CreateSquadStudent(int studentId, int squadId);
    }
}
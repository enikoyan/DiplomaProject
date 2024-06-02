using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IStudentService
    {
        Task<Student> CreateStudent(Student student);
        Task DeleteStudent(int studentId);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(int studentId);
        Task<List<Student>> GetStudentsByCourse(string courseName);
        Task<List<Student>> GetStudentsBySquad(string squadName);
    }
}
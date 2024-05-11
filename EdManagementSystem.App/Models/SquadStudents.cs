using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.App.Models
{
    public class SquadStudents
    {
        public string? SquadName { get; set; }
        public List<Student>? Students { get; set; }
    }
}

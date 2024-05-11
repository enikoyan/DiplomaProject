using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.App.Models
{
    public class AttendanceViewModel
    {
        public required List<Squad> squadsList {  get; set; }
        public List<SquadStudents>? studentsList { get; set; }
    }
}

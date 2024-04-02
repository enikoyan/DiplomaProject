using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.App.Models
{
    public class MaterialsPageViewModel
    {
        public required List<Course> coursesList { get; set; }
        public required List<Squad> squadsList { get; set; }
    }
}
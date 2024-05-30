using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.DTO
{
    public class HomeworkDTO
    {
        public required Homework Homework { get; set; }
        public List<Models.File>? AttachedFiles { get; set; }
    }
}

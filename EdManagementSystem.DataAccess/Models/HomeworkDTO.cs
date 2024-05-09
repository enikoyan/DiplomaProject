namespace EdManagementSystem.DataAccess.Models
{
    public class HomeworkDTO
    {
        public required Homework Homework { get; set; }
        public List<File>? AttachedFiles { get; set; }
    }
}

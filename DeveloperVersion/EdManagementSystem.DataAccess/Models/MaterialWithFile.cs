namespace EdManagementSystem.DataAccess.Models
{
    public class MaterialWithFile
    {
        public int Id { get; set; }
        public Guid IdFile { get; set; }
        public int IdCourse { get; set; }
        public int? IdSquad { get; set; }

        public File File { get; set; }

    }
}

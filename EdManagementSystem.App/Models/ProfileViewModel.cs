using System.ComponentModel.DataAnnotations;

namespace EdManagementSystem.App.Models
{
    public class ProfileViewModel
    {
        public string Fio { get; set; }
        public string Post { get; set; }
        public double Rate { get; set; }
        public string Place { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? Experience { get; set; }
        public string RegDate { get; set; }
        public int StudentsCount { get; set; }
        public int SquadsCount { get; set; }
        public List<string> Squads { get; set; }
        public List<string> Courses { get; set; }
        public List<List<string>> SocialMediaList { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models
{
    [Index("IdTeacher", Name = "id_teacher")]
    public partial class SocialMedium
    {
        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }

        [Column("id_teacher", TypeName = "int(11)")]
        public int IdTeacher { get; set; }

        [Column("socialMedia_name", TypeName = "enum('Telegram','Vk','Discord','Facebook')")]
        public string SocialMediaName { get; set; } = null!;

        [Column("socialMedia_url")]
        [StringLength(2083)]
        public string SocialMediaUrl { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("IdTeacher")]
        [InverseProperty("SocialMedia")]
        public virtual Teacher? IdTeacherNavigation { get; set; }
    }
}

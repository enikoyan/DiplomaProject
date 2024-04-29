using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models
{
    [Table("Schedule")]
    [Index("SquadId", Name = "squad_id")]
    [Index("TeacherId", Name = "teacher_id")]
    public partial class Schedule
    {
        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }

        [Column("teacher_id", TypeName = "int(11)")]
        public int TeacherId { get; set; }

        [Column("squad_id", TypeName = "int(11)")]
        public int SquadId { get; set; }

        [Column("weekday", TypeName = "enum('Понедельник','Вторник','Среда','Четверг','Пятница','Суббота','Воскресенье')")]
        public string Weekday { get; set; } = null!;

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("timeline_start", TypeName = "time")]
        public TimeOnly TimelineStart { get; set; }

        [Column("timeline_end", TypeName = "time")]
        public TimeOnly TimelineEnd { get; set; }

        [Column("place")]
        [StringLength(255)]
        public string Place { get; set; } = null!;

        [Column("note")]
        [StringLength(255)]
        public string? Note { get; set; }

        [NotMapped]
        public string? SquadName { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("SquadId")]
        [InverseProperty("Schedules")]
        public virtual Squad? Squad { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("TeacherId")]
        [InverseProperty("Schedules")]
        public virtual Teacher? Teacher { get; set; } = null!;
    }
}

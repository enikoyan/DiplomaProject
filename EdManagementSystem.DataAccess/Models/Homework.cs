using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models;

[Table("Homework")]
[Index("CourseId", Name = "course_id")]
[Index("HomeworkId", Name = "homework_id")]
[Index("SquadId", Name = "squad_id")]
public partial class Homework
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("homework_id")]
    public Guid HomeworkId { get; set; }

    [Column("course_id", TypeName = "int(11)")]
    public int CourseId { get; set; }

    [Column("squad_id", TypeName = "int(11)")]
    public int? SquadId { get; set; }

    [Column("date_added")]
    public DateTime DateAdded { get; set; }

    [Column("deadline")]
    public DateTime? Deadline { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description")]
    [StringLength(1024)]
    public string? Description { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CourseId")]
    [InverseProperty("Homeworks")]
    public virtual Course? Course { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("SquadId")]
    [InverseProperty("Homeworks")]
    public virtual Squad? Squad { get; set; } = null!;
}

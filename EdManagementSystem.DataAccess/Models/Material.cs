using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdManagementSystem.DataAccess.Models;

[Table("Material")]
[Index("IdCourse", Name = "id_course")]
[Index("IdSquad", Name = "id_squad")]
public partial class Material
{
    [Key]
    [Column("material_id", TypeName = "int(11)")]
    public int MaterialId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("date_added")]
    public DateTime DateAdded { get; set; }

    [Column("type")]
    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column("id_course", TypeName = "int(11)")]
    public int IdCourse { get; set; }

    [Column("id_squad", TypeName = "int(11)")]
    public int? IdSquad { get; set; }

    [ForeignKey("IdCourse")]
    [InverseProperty("Materials")]
    public virtual Course IdCourseNavigation { get; set; } = null!;

    [ForeignKey("IdSquad")]
    [InverseProperty("Materials")]
    public virtual Squad? IdSquadNavigation { get; set; } = null!;
}
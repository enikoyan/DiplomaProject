using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models;

[Table("Material")]
[Index("IdCourse", Name = "id_course")]
[Index("IdSquad", Name = "id_squad")]
[Index("MaterialId", Name = "material_id", IsUnique = true)]
public partial class Material
{
    [JsonIgnore]
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [JsonIgnore]
    [Column("material_id")]
    public Guid MaterialId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("date_added")]
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    [Column("type")]
    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column("id_course", TypeName = "int(11)")]
    public int IdCourse { get; set; }

    [Column("id_squad", TypeName = "int(11)")]
    [DefaultValue(null)]
    public int? IdSquad { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdCourse")]
    [InverseProperty("Materials")]
    public virtual Course? IdCourseNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdSquad")]
    [InverseProperty("Materials")]
    public virtual Squad? IdSquadNavigation { get; set; } = null!;
}

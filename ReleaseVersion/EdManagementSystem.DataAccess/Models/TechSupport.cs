using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models;

[Table("TechSupport")]
[Index("IdUser", Name = "id_user")]
public partial class TechSupport
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("id_user", TypeName = "int(11)")]
    public int IdUser { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column("date_creation")]
    public DateTime DateCreation { get; set; }

    [Column("status", TypeName = "enum('в обработке','обработано')")]
    public string Status { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdUser")]
    [InverseProperty("TechSupports")]
    public virtual User? IdUserNavigation { get; set; }
}

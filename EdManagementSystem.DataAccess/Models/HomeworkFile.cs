using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models;

[Table("Homework_File")]
[Index("FileId", Name = "file_id")]
[Index("HomeworkId", Name = "homework_id")]
public partial class HomeworkFile
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("file_id")]
    public Guid FileId { get; set; }

    [Column("homework_id", TypeName = "int(11)")]
    public int HomeworkId { get; set; }

    [JsonIgnore]
    [ForeignKey("FileId")]
    [InverseProperty("HomeworkFiles")]
    public virtual File File { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("HomeworkId")]
    [InverseProperty("HomeworkFiles")]
    public virtual Homework Homework { get; set; } = null!;
}

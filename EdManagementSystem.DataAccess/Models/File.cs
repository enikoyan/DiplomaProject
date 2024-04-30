using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("File")]
public partial class File
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("file_id")]
    public Guid FileId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("type")]
    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column("date_added")]
    public DateTime DateAdded { get; set; }

    [InverseProperty("File")]
    public virtual ICollection<HomeworkFile> HomeworkFiles { get; set; } = new List<HomeworkFile>();
}

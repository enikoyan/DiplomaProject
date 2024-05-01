using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("File")]
public partial class File
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("type")]
    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column("date_added")]
    public DateTime DateAdded { get; set; }

    [JsonIgnore]
    [InverseProperty("File")]
    public virtual ICollection<HomeworkFile> HomeworkFiles { get; set; } = new List<HomeworkFile>();

    [JsonIgnore]
    [InverseProperty("IdFileNavigation")]
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
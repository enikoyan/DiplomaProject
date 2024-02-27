using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("Teacher")]
public partial class Teacher
{
    [Key]
    [Column("teacher_id", TypeName = "int(11)")]
    public int TeacherId { get; set; }

    [Column("fio")]
    [StringLength(255)]
    public string Fio { get; set; } = null!;

    [Column("avatar", TypeName = "text")]
    public string? Avatar { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("post")]
    [StringLength(255)]
    public string Post { get; set; } = null!;

    [Column("rate")]
    public double Rate { get; set; }

    [Column("phone_number")]
    [StringLength(11)]
    public string? PhoneNumber { get; set; }

    [Column("experience", TypeName = "int(11)")]
    public int? Experience { get; set; }

    [Column("regDate")]
    public DateTime RegDate { get; set; }

    [JsonIgnore]
    [ForeignKey("TeacherId")]
    [InverseProperty("Teacher")]
    public virtual User? TeacherNavigation { get; set; }

    [InverseProperty("IdTeacherNavigation")]
    public virtual ICollection<SocialMedium> SocialMedia { get; set; } = new List<SocialMedium>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("Course")]
[Index("CourseTutor", Name = "course_tutor")]
public partial class Course
{
    [Key]
    [Column("course_id", TypeName = "int(11)")]
    public int CourseId { get; set; }

    [Column("course_name")]
    [StringLength(255)]
    public string CourseName { get; set; } = null!;

    [Column("option_value")]
    [StringLength(255)]
    public string OptionValue { get; set; } = null!;

    [Column("course_addDate")]
    public DateTime CourseAddDate { get; set; }

    [Column("course_tutor", TypeName = "int(11)")]
    public int CourseTutor { get; set; }

    [JsonIgnore]
    [InverseProperty("Course")]
    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    [JsonIgnore]
    [InverseProperty("IdCourseNavigation")]
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    [JsonIgnore]
    [InverseProperty("IdCourseNavigation")]
    public virtual ICollection<Squad> Squads { get; set; } = new List<Squad>();

    [JsonIgnore]
    [ForeignKey("CourseTutor")]
    [InverseProperty("Courses")]
    public virtual Teacher CourseTutorNavigation { get; set; } = null!;
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Column("course_addDate", TypeName = "int(11)")]
    public int CourseAddDate { get; set; }

    [Column("course_tutor", TypeName = "int(11)")]
    public int CourseTutor { get; set; }

    [InverseProperty("IdCourseNavigation")]
    public virtual ICollection<Squad> Squads { get; set; } = new List<Squad>();
}
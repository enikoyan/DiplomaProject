using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("Student")]
public partial class Student
{
    [Key]
    [Column("student_id", TypeName = "int(11)")]
    public int StudentId { get; set; }

    [Column("fio")]
    [StringLength(255)]
    public string Fio { get; set; } = null!;

    [Column("rate")]
    public double Rate { get; set; }

    [Column("birthDate")]
    public DateTime BirthDate { get; set; }

    [InverseProperty("IdStudentNavigation")]
    public virtual ICollection<SquadStudent> SquadStudents { get; set; } = new List<SquadStudent>();
}

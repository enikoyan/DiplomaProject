using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly BirthDate { get; set; }

    [JsonIgnore]
    [InverseProperty("IdStudentNavigation")]
    public virtual ICollection<SquadStudent> SquadStudents { get; set; } = new List<SquadStudent>();
}

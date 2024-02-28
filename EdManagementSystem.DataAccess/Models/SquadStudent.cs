using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("SquadStudent")]
[Index("IdSquad", Name = "id_squad")]
[Index("IdStudent", Name = "id_student")]
public partial class SquadStudent
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("id_student", TypeName = "int(11)")]
    public int IdStudent { get; set; }

    [Column("id_squad", TypeName = "int(11)")]
    public int IdSquad { get; set; }

    [Column("attachedDate")]
    public DateTime AttachedDate { get; set; }

    [JsonIgnore]
    [ForeignKey("IdSquad")]
    [InverseProperty("SquadStudents")]
    public virtual Squad? IdSquadNavigation { get; set; }

    [JsonIgnore]
    [ForeignKey("IdStudent")]
    [InverseProperty("SquadStudents")]
    public virtual Student? IdStudentNavigation { get; set; }
}

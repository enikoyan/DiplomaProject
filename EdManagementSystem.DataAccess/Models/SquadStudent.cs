using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Keyless]
[Table("SquadStudent")]
[Index("IdSquad", Name = "id_squad")]
[Index("IdStudent", Name = "id_student")]
public partial class SquadStudent
{
    [Column("id_squad", TypeName = "int(11)")]
    public int IdSquad { get; set; }

    [Column("id_student", TypeName = "int(11)")]
    public int IdStudent { get; set; }

    [ForeignKey("IdSquad")]
    public virtual Squad IdSquadNavigation { get; set; } = null!;

    [ForeignKey("IdStudent")]
    public virtual Student IdStudentNavigation { get; set; } = null!;
}

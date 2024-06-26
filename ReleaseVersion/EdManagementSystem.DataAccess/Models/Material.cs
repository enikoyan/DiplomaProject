﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models;

[Table("Material")]
[Index("IdCourse", Name = "id_course")]
[Index("IdFile", Name = "id_file")]
[Index("IdSquad", Name = "id_squad")]
public partial class Material
{
    [JsonIgnore]
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("id_file")]
    public Guid IdFile { get; set; }

    [Column("id_course", TypeName = "int(11)")]
    public int IdCourse { get; set; }

    [Column("id_squad", TypeName = "int(11)")]
    public int? IdSquad { get; set; }

    [JsonIgnore]
    [ForeignKey("IdCourse")]
    [InverseProperty("Materials")]
    public virtual Course IdCourseNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdFile")]
    [InverseProperty("Materials")]
    public virtual File IdFileNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdSquad")]
    [InverseProperty("Materials")]
    public virtual Squad? IdSquadNavigation { get; set; }
}

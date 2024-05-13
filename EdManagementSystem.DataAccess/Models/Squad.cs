using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("Squad")]
[Index("IdCourse", Name = "id_course")]
public partial class Squad
{
    [Key]
    [Column("squad_id", TypeName = "int(11)")]
    public int SquadId { get; set; }

    [Column("squadName")]
    [StringLength(255)]
    public string SquadName { get; set; } = null!;

    [Column("option_value")]
    [StringLength(255)]
    public string OptionValue { get; set; } = null!;

    [Column("addDate")]
    public DateTime AddDate { get; set; }

    [Column("id_course", TypeName = "int(11)")]
    public int IdCourse { get; set; }

    [JsonIgnore]
    [ForeignKey("IdCourse")]
    [InverseProperty("Squads")]
    public virtual Course? IdCourseNavigation { get; set; }

    [JsonIgnore]
    [InverseProperty("Squad")]
    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    [JsonIgnore]
    [InverseProperty("IdSquadNavigation")]
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    [JsonIgnore]
    [InverseProperty("Squad")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [JsonIgnore]
    [InverseProperty("IdSquadNavigation")]
    public virtual ICollection<SquadStudent> SquadStudents { get; set; } = new List<SquadStudent>();

    [JsonIgnore]
    [InverseProperty("Squad")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}

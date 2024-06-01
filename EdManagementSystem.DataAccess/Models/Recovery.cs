using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("Recovery")]
[Index("UserId", Name = "user_id")]
public partial class Recovery
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("confirmed")]
    public bool Confirmed { get; set; } = false;

    [Column("user_key")]
    public Guid UserKey { get; set; }

    [Column("expire_time", TypeName = "time")]
    public TimeOnly ExpireTime { get; set; }

    [Column("server_key")]
    public Guid ServerKey { get; set; }

    [JsonIgnore]
    [ForeignKey("UserId")]
    [InverseProperty("Recoveries")]
    public virtual User User { get; set; } = null!;
}

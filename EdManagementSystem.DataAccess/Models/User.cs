using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("user_email")]
    [StringLength(255)]
    public string UserEmail { get; set; } = null!;

    [Column("user_password")]
    [StringLength(60)]
    public string UserPassword { get; set; } = null!;

    [Column("user_role", TypeName = "enum('teacher','admin')")]
    public string UserRole { get; set; } = null!;
}

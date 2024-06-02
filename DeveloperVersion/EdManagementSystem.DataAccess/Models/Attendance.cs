using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace EdManagementSystem.DataAccess.Models
{
    [Table("Attendance")]
    [Index("FileId", Name = "file_id")]
    [Index("SquadId", Name = "squad_id")]
    public partial class Attendance
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("added_date")]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [Column("squad_id", TypeName = "int(11)")]
        public int SquadId { get; set; }

        [Column("week_date")]
        [StringLength(8)]
        public string WeekDate { get; set; } = null!;

        [Column("file_id")]
        public Guid FileId { get; set; }

        [JsonIgnore]
        [ForeignKey("FileId")]
        [InverseProperty("Attendances")]
        public virtual File File { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("SquadId")]
        [InverseProperty("Attendances")]
        public virtual Squad Squad { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EducationalSystem.Domain.Entities
{
    public class Attendance
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; } = string.Empty;
        public virtual ApplicationUser Student { get; set; }

        [Required]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }

        [ForeignKey("MarkedByUser")]
        public string? MarkedBy { get; set; }
        public virtual ApplicationUser? MarkedByUser { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime MarkedAt { get; set; } = DateTime.UtcNow;
    }

    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late
    }
}


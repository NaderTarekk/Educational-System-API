using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationalSystem.Domain.Entities
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(120)]
        public string? InstructorName { get; set; }

        [ForeignKey("Assistant")]
        public string? AssistantId { get; set; } = "";
        public virtual ApplicationUser? Assistant { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

        public int Capacity { get; set; } = 0;


        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; } // اليوم (أحد، اتنين، إلخ)

        [Required]
        public TimeSpan StartTime { get; set; } // وقت البداية

        [Required]
        [Range(1, 12)]
        public int DurationInHours { get; set; } // المدة بالساعات

        [MaxLength(200)]
        public string? Location { get; set; }
        // Property محسوبة لوقت الانتهاء
        [NotMapped]
        public TimeSpan EndTime
        {
            get { return StartTime.Add(TimeSpan.FromHours(DurationInHours)); }
        }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal FeesPerLesson { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

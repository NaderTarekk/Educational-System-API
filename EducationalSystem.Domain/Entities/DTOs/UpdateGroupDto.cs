using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EducationalSystem.Domain.Entities.DTOs
{
    public class UpdateGroupDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string? InstructorName { get; set; }

        public string? AssistantId { get; set; }
        public virtual ApplicationUser? Assistant { get; set; }

        public ICollection<ApplicationUser>? Students { get; set; } = new List<ApplicationUser>();


        public int Capacity { get; set; } = 0;

        public DateTime? StartDate { get; set; }

        public decimal FeesPerLesson { get; set; }

        public bool IsActive { get; set; } = true;

        public DayOfWeek DayOfWeek { get; set; } 

        public TimeSpan StartTime { get; set; }
        public string? Location { get; set; }
        public int DurationInHours { get; set; } 
    }
}

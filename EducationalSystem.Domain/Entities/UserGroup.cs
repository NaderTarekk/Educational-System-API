using System.Text.Json.Serialization;

namespace EducationalSystem.Domain.Entities
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}

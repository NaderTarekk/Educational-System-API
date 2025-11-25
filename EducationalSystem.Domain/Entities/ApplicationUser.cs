using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace EducationalSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

    }
}

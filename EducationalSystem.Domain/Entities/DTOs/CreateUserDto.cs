namespace EducationalSystem.Domain.Entities.DTOs
{
    public class CreateUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "Student";
        public List<Guid> GroupIds { get; set; } = new List<Guid>();
    }
}

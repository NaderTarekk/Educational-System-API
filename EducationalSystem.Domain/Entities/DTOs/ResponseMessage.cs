namespace EducationalSystem.Domain.Entities.DTOs
{
    public class ResponseMessage
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string? token { get; set; }
        public string? role { get; set; }
        public ApplicationUser? user { get; set; }
    }
}

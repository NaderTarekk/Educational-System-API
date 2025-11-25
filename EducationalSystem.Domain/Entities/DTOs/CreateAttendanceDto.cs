namespace EducationalSystem.Domain.Entities.DTOs
{
    public class CreateAttendanceDto
    {
        public string StudentId { get; set; } = string.Empty;

        public Guid GroupId { get; set; }

        public DateTime Date { get; set; }

        public AttendanceStatus Status { get; set; }

        public string? MarkedBy { get; set; }
    }
}

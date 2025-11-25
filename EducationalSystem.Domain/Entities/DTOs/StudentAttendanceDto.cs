namespace EducationalSystem.Domain.Entities.DTOs
{
    public class StudentAttendanceDto
    {
        public string StudentId { get; set; } = string.Empty;
        public AttendanceStatus Status { get; set; }
    }
}

namespace EducationalSystem.Domain.Entities.DTOs
{
    public class AttendanceReportDto
    {
        public Guid? GroupId { get; set; }
        public string? StudentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

namespace EducationalSystem.Domain.Entities.DTOs
{
    public class BulkAttendanceDto
    {
        public Guid GroupId { get; set; }
        public DateTime Date { get; set; }
        public string MarkedBy { get; set; } = string.Empty;
        public List<StudentAttendanceDto> Attendances { get; set; } = new();
    }

}

namespace EducationalSystem.Domain.Entities.DTOs
{
    public class CreateExpenseDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = "Other";
        public string PaidBy { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Reference { get; set; }
    }

}

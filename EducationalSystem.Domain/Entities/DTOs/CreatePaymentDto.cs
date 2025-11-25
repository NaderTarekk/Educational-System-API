namespace EducationalSystem.Domain.Entities.DTOs
{
    public class CreatePaymentDto
    {
        public Guid Id { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public Guid GroupId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "ج.م";

        public PaymentMethod Method { get; set; }

        public string? Reference { get; set; } 

        public string ReceivedBy { get; set; } = string.Empty;

    }
}

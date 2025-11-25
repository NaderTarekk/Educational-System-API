using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EducationalSystem.Domain.Entities
{

    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        [Required, ForeignKey("Student")]
        public string StudentId { get; set; } = string.Empty;
        public virtual ApplicationUser Student { get; set; }

        [Required, ForeignKey("Group")]
        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [MaxLength(3)]
        public string Currency { get; set; } = "ج.م";

        [Required]
        public PaymentMethod Method { get; set; }

        [MaxLength(80)]
        public string? Reference { get; set; }  // رقم التحويل أو الفاتورة

        [Required, ForeignKey("ReceivedByUser")]
        public string ReceivedBy { get; set; } = string.Empty;
        public virtual ApplicationUser ReceivedByUser { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        Transfer,
        Wallet
    }

}


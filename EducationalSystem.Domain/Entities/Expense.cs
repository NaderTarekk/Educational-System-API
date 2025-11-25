using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalSystem.Domain.Entities
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        public string Category { get; set; } = "Other";

        [Required, ForeignKey("PaidByUser")]
        public string PaidBy { get; set; } = string.Empty;
        public virtual ApplicationUser PaidByUser { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        [MaxLength(100)]
        public string? Reference { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IPayment
    {
        Task<ResponseMessage> CreatePaymentAsync(CreatePaymentDto payment);
        Task<GetByIdResponseDto<List<Payment>>> GetPaymentsAsync();
        Task<GetByIdResponseDto<Payment>> GetPaymentByIdAsync(string? id);
        Task<GetByIdResponseDto<List<Payment>>> GetPaymentsByStudentIdAsync(string? studentId);
        Task<ResponseMessage> UpdatePaymentAsync(CreatePaymentDto payment);
        Task<ResponseMessage> DeletePaymentAsync(string id, string deletedById);
        Task<GetByIdResponseDto<List<Payment>>> GetPaymentsByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}

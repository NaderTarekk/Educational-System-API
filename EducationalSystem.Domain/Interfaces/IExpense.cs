using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IExpense
    {
        Task<ResponseMessage> CreateExpenseAsync(CreateExpenseDto expense);
        Task<GetByIdResponseDto<List<Expense>>> GetAllExpensesAsync();
        Task<GetByIdResponseDto<Expense>> GetExpenseByIdAsync(string id);
        Task<ResponseMessage> UpdateExpenseAsync(CreateExpenseDto expense);
        Task<ResponseMessage> DeleteExpenseAsync(string id);
        Task<GetByIdResponseDto<List<Expense>>> GetExpensesByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}

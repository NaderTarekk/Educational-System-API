using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class ExpenseRepo : IExpense
    {
        private readonly EducationalSystemApiContext _ctx;

        public ExpenseRepo(EducationalSystemApiContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ResponseMessage> CreateExpenseAsync(CreateExpenseDto expense)
        {
            try
            {
                var newExpense = new Expense
                {
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    PaidBy = expense.PaidBy,
                    Date = expense.Date,
                    Reference = expense.Reference,
                    CreatedAt = DateTime.UtcNow
                };

                await _ctx.TbExpenses.AddAsync(newExpense);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تسجيل المصروف بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء تسجيل المصروف" };
            }
        }

        public async Task<GetByIdResponseDto<List<Expense>>> GetAllExpensesAsync()
        {
            try
            {
                var expenses = await _ctx.TbExpenses
                    .AsNoTracking()
                    .Include(e => e.PaidByUser)
                    .OrderByDescending(e => e.Date)
                    .ToListAsync();

                return new GetByIdResponseDto<List<Expense>>
                {
                    Success = true,
                    Message = "تم جلب جميع المصروفات بنجاح",
                    Data = expenses
                };
            }
            catch
            {
                return new GetByIdResponseDto<List<Expense>>
                {
                    Success = false,
                    Message = "حدث خطأ أثناء جلب المصروفات",
                    Data = new List<Expense>()
                };
            }
        }

        public async Task<GetByIdResponseDto<Expense>> GetExpenseByIdAsync(string id)
        {
            try
            {
                var expense = await _ctx.TbExpenses
                    .Include(e => e.PaidByUser)
                    .FirstOrDefaultAsync(e => e.Id.ToString() == id);

                if (expense == null)
                    return new GetByIdResponseDto<Expense> { Success = false, Message = "المصروف غير موجود", Data = new Expense() };

                return new GetByIdResponseDto<Expense> { Success = true, Message = "تم جلب بيانات المصروف", Data = expense };
            }
            catch
            {
                return new GetByIdResponseDto<Expense> { Success = false, Message = "حدث خطأ أثناء جلب المصروف", Data = new Expense() };
            }
        }

        public async Task<ResponseMessage> UpdateExpenseAsync(CreateExpenseDto expense)
        {
            try
            {
                var existing = await _ctx.TbExpenses.FindAsync(expense.Id);
                if (existing == null)
                    return new ResponseMessage { success = false, message = "المصروف غير موجود" };

                existing.Description = expense.Description;
                existing.Amount = expense.Amount;
                existing.Category = expense.Category;
                existing.PaidBy = expense.PaidBy;
                existing.Date = expense.Date;
                existing.Reference = expense.Reference;

                _ctx.TbExpenses.Update(existing);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تحديث المصروف بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدث خطأ أثناء تحديث المصروف" };
            }
        }

        public async Task<ResponseMessage> DeleteExpenseAsync(string id)
        {
            try
            {
                var expense = await _ctx.TbExpenses.FirstOrDefaultAsync(e => e.Id.ToString() == id);
                if (expense == null)
                    return new ResponseMessage { success = false, message = "المصروف غير موجود" };

                _ctx.TbExpenses.Remove(expense);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم حذف المصروف بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدث خطأ أثناء حذف المصروف" };
            }
        }

        public async Task<GetByIdResponseDto<List<Expense>>> GetExpensesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var expenses = await _ctx.TbExpenses
                    .AsNoTracking()
                    .Where(e => e.Date >= fromDate && e.Date <= toDate)
                    .OrderByDescending(e => e.Date)
                    .ToListAsync();

                return new GetByIdResponseDto<List<Expense>>
                {
                    Success = true,
                    Message = "تم جلب المصروفات حسب التاريخ بنجاح",
                    Data = expenses
                };
            }
            catch
            {
                return new GetByIdResponseDto<List<Expense>>
                {
                    Success = false,
                    Message = "حدث خطأ أثناء جلب المصروفات حسب التاريخ",
                    Data = new List<Expense>()
                };
            }
        }
    }
}
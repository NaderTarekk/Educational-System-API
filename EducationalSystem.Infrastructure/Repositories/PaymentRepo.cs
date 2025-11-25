using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class PaymentRepo : IPayment
    {
        private readonly EducationalSystemApiContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentRepo(EducationalSystemApiContext ctx, UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task<ResponseMessage> CreatePaymentAsync(CreatePaymentDto payment)
        {
            try
            {
                bool alreadyExists = await _ctx.TbPayments
            .AnyAsync(p =>
                p.StudentId == payment.StudentId &&
                p.GroupId == payment.GroupId &&
                p.CreatedAt.Date == DateTime.UtcNow.Date
            );

                if (alreadyExists)
                {
                    return new ResponseMessage
                    {
                        success = false,
                        message = "تم تسجيل دفعة لهذا الطالب مسبقًا في هذا اليوم."
                    };
                }
                var newPayment = new Payment
                {
                    StudentId = payment.StudentId,
                    GroupId = payment.GroupId,
                    Amount = payment.Amount,
                    Method = payment.Method,
                    Reference = GenerateReference(),
                    ReceivedBy = payment.ReceivedBy,
                    CreatedAt = DateTime.UtcNow
                };

                await _ctx.TbPayments.AddAsync(newPayment);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تسجيل الدفعة بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء تسجيل الدفعة" };
            }
        }

        public async Task<GetByIdResponseDto<List<Payment>>> GetPaymentsAsync()
        {
            try
            {
                var query = _ctx.TbPayments.AsQueryable();

                var payments = await query.AsNoTracking().ToListAsync();

                return new GetByIdResponseDto<List<Payment>> { Success = true, Message = "تم جلب الدفعات بنجاح", Data = payments };
            }
            catch
            {
                return new GetByIdResponseDto<List<Payment>> { Success = false, Message = "حدثت مشكلة أثناء جلب الدفعات", Data = new List<Payment>() };
            }
        }

        public async Task<ResponseMessage> UpdatePaymentAsync(CreatePaymentDto payment)
        {
            try
            {
                var existing = await _ctx.TbPayments
                    .FindAsync(payment.Id);
                if (existing == null)
                    return new ResponseMessage { success = false, message = "الدفعة غير موجودة" };

                existing.Amount = payment.Amount;
                existing.Method = payment.Method;
                existing.Reference = payment.Reference;
                existing.ReceivedBy = payment.ReceivedBy;
                existing.Currency = payment.Currency;
                existing.Reference = payment.Reference;
                existing.Method = payment.Method;

                _ctx.TbPayments.Update(existing);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تحديث بيانات الدفعة بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء تحديث بيانات الدفعة" };
            }
        }

        public async Task<ResponseMessage> DeletePaymentAsync(string id, string deletedById)
        {
            try
            {
                var payment = _ctx.TbPayments.Find(Guid.Parse(id));
                if (payment == null)
                    return new ResponseMessage { success = false, message = "الدفعة غير موجودة" };

                var existDeletedBy = await _userManager.FindByIdAsync(deletedById);
                if (existDeletedBy==null)
                    return new ResponseMessage { success = false, message = "مُنفِّذ عملية الحذف غير موجود" };



                _ctx.TbPayments.Remove(payment);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم حذف الدفعة بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء حذف الدفعة" };
            }
        }

        public async Task<GetByIdResponseDto<Payment>> GetPaymentByIdAsync(string? id)
        {
            try
            {
                var payment = await _ctx.TbPayments
                    .AsNoTracking()
                    .Include(p => p.Student)
                    .Include(p => p.Group)
                    .Include(p => p.ReceivedByUser)
                    .FirstOrDefaultAsync(p => p.Id.ToString() == id);

                if (payment == null)
                    return new GetByIdResponseDto<Payment> { Success = false, Message = "الدفعة غير موجودة", Data = new Payment() };

                return new GetByIdResponseDto<Payment> { Success = true, Message = "تم جلب بيانات الدفعة بنجاح", Data = payment };
            }
            catch
            {
                return new GetByIdResponseDto<Payment> { Success = false, Message = "حدثت مشكلة أثناء جلب بيانات الدفعة", Data = new Payment() };
            }
        }

        public async Task<GetByIdResponseDto<List<Payment>>> GetPaymentsByStudentIdAsync(string? studentId)
        {
            try
            {
                var payments = await _ctx.TbPayments
                    .AsNoTracking()
                    .Include(p => p.Group)
                    .Include(p => p.ReceivedByUser)
                    .Where(p => p.StudentId == studentId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                if (payments == null || payments.Count == 0)
                    return new GetByIdResponseDto<List<Payment>> { Success = false, Message = "لا توجد دفعات لهذا الطالب", Data = new List<Payment>() };

                return new GetByIdResponseDto<List<Payment>> { Success = true, Message = "تم جلب دفعات الطالب بنجاح", Data = payments };
            }
            catch
            {
                return new GetByIdResponseDto<List<Payment>> { Success = false, Message = "حدثت مشكلة أثناء جلب دفعات الطالب", Data = new List<Payment>() };
            }
        }

        public async Task<GetByIdResponseDto<List<Payment>>> GetPaymentsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var payments = await _ctx.TbPayments
                    .AsNoTracking()
                    .Include(p => p.Student)
                    .Include(p => p.Group)
                    .Include(p => p.ReceivedByUser)
                    .Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync(); 

                return new GetByIdResponseDto<List<Payment>> { Success = true, Message = "تم جلب الدفعات في النطاق الزمني بنجاح", Data = payments };
            }
            catch
            {
                return new GetByIdResponseDto<List<Payment>> { Success = false, Message = "حدث خطأ أثناء جلب الدفعات", Data = new List<Payment>() };
            }
        }

        private string GenerateReference()
        {
            return $"NHC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }
    }
}

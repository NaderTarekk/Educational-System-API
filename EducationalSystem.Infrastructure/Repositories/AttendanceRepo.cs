// AttendanceRepo.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class AttendanceRepo : IAttendance
    {
        private readonly EducationalSystemApiContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceRepo(EducationalSystemApiContext ctx, UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        // تسجيل حضور جماعي 
        // AttendanceRepo.cs
        public async Task<ResponseMessage> CreateBulkAttendanceAsync(BulkAttendanceDto dto)
        {
            try
            {
                // التحقق من وجود المجموعة
                var groupExists = await _ctx.TbGroups.AnyAsync(g => g.Id == dto.GroupId);
                if (!groupExists)
                    return new ResponseMessage { success = false, message = "المجموعة غير موجودة" };

                // التحقق من وجود المستخدم الذي يسجل الحضور
                if (!string.IsNullOrEmpty(dto.MarkedBy))
                {
                    var userExists = await _userManager.FindByIdAsync(dto.MarkedBy);
                    if (userExists == null)
                        return new ResponseMessage { success = false, message = "المستخدم غير موجود" };
                }

                // حذف السجلات القديمة لنفس المجموعة والتاريخ
                var existingRecords = await _ctx.TbAttendance
                    .Where(a => a.GroupId == dto.GroupId && a.Date.Date == dto.Date.Date)
                    .ToListAsync();

                if (existingRecords.Any())
                    _ctx.TbAttendance.RemoveRange(existingRecords);

                // التحقق من وجود الطلاب
                var studentIds = dto.Attendances.Select(a => a.StudentId).ToList();
                var existingStudents = await _userManager.Users
                    .Where(u => studentIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync();

                // فلترة الطلاب الموجودين فقط
                var validAttendances = dto.Attendances
                    .Where(a => existingStudents.Contains(a.StudentId))
                    .ToList();

                if (!validAttendances.Any())
                    return new ResponseMessage { success = false, message = "لا يوجد طلاب صالحون للتسجيل" };

                // إضافة السجلات الجديدة
                var attendanceRecords = validAttendances.Select(a => new Attendance
                {
                    Id = Guid.NewGuid(),
                    StudentId = a.StudentId,
                    GroupId = dto.GroupId,
                    Date = dto.Date.Date,
                    Status = a.Status,
                    MarkedBy = string.IsNullOrEmpty(dto.MarkedBy) ? null : dto.MarkedBy,
                    MarkedAt = DateTime.UtcNow
                }).ToList();

                await _ctx.TbAttendance.AddRangeAsync(attendanceRecords);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage
                {
                    success = true,
                    message = $"تم تسجيل حضور {validAttendances.Count} طالب بنجاح"
                };
            }
            catch (Exception ex)
            {
                // سجل التفاصيل الكاملة للخطأ
                var innerException = ex.InnerException?.Message ?? ex.Message;
                return new ResponseMessage
                {
                    success = false,
                    message = $"حدث خطأ: {innerException}"
                };
            }
        }

        // تحديث حضور طالب واحد
        public async Task<ResponseMessage> UpdateAttendanceAsync(CreateAttendanceDto attendance)
        {
            try
            {
                var existing = await _ctx.TbAttendance
                    .FirstOrDefaultAsync(a => a.StudentId == attendance.StudentId
                                           && a.GroupId == attendance.GroupId
                                           && a.Date.Date == attendance.Date.Date);

                if (existing == null)
                {
                    // إنشاء سجل جديد
                    var newRecord = new Attendance
                    {
                        StudentId = attendance.StudentId,
                        GroupId = attendance.GroupId,
                        Date = attendance.Date.Date,
                        Status = attendance.Status,
                        MarkedBy = attendance.MarkedBy,
                        MarkedAt = DateTime.UtcNow
                    };
                    await _ctx.TbAttendance.AddAsync(newRecord);
                }
                else
                {
                    existing.Status = attendance.Status;
                    existing.MarkedBy = attendance.MarkedBy;
                    existing.MarkedAt = DateTime.UtcNow;
                    _ctx.TbAttendance.Update(existing);
                }

                await _ctx.SaveChangesAsync();
                return new ResponseMessage { success = true, message = "تم تحديث الحضور بنجاح" };
            }
            catch
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء تحديث الحضور" };
            }
        }

        // جلب الحضور حسب المجموعة والتاريخ
        public async Task<GetByIdResponseDto<List<Attendance>>> GetAttendancesByGroupAndDateAsync(Guid groupId, DateTime date)
        {
            try
            {
                var attendances = await _ctx.TbAttendance
                    .Include(a => a.Student)
                    .Include(a => a.Group)
                    .Where(a => a.GroupId == groupId && a.Date.Date == date.Date)
                    .OrderBy(a => a.Student.FirstName)
                    .ToListAsync();

                return new GetByIdResponseDto<List<Attendance>>
                {
                    Success = true,
                    Message = "تم جلب سجلات الحضور",
                    Data = attendances
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<List<Attendance>>
                {
                    Success = false,
                    Message = $"حدث خطأ: {ex.Message}",
                    Data = new List<Attendance>()
                };
            }
        }

        // جلب الحضور حسب الطالب
        // AttendanceRepo.cs
        // AttendanceRepo.cs
        public async Task<GetByIdResponseDto<List<Attendance>>> GetAttendancesByStudentIdAsync(
            string studentId, Guid? groupId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // 📝 Logging
                Console.WriteLine($"📥 Request params:");
                Console.WriteLine($"   StudentId: {studentId}");
                Console.WriteLine($"   GroupId: {groupId}");
                Console.WriteLine($"   StartDate: {startDate} (Kind: {startDate?.Kind})");
                Console.WriteLine($"   EndDate: {endDate} (Kind: {endDate?.Kind})");

                var query = _ctx.TbAttendance
                    .Include(a => a.Group)
                    .Include(a => a.Student)
                    .Where(a => a.StudentId == studentId);

                if (groupId.HasValue)
                    query = query.Where(a => a.GroupId == groupId.Value);

                // ✅ الحل: تحويل لـ Local date وإزالة الوقت تماماً
                if (startDate.HasValue)
                {
                    // تحويل من UTC لـ Local إذا لزم
                    var localStart = startDate.Value.Kind == DateTimeKind.Utc
                        ? startDate.Value.ToLocalTime()
                        : startDate.Value;

                    var startDateOnly = localStart.Date;

                    Console.WriteLine($"   📅 Filtering Date >= {startDateOnly:yyyy-MM-dd}");
                    query = query.Where(a => a.Date >= startDateOnly);
                }

                if (endDate.HasValue)
                {
                    var localEnd = endDate.Value.Kind == DateTimeKind.Utc
                        ? endDate.Value.ToLocalTime()
                        : endDate.Value;

                    var endDateOnly = localEnd.Date;

                    Console.WriteLine($"   📅 Filtering Date <= {endDateOnly:yyyy-MM-dd}");
                    query = query.Where(a => a.Date <= endDateOnly);
                }

                var attendances = await query
                    .OrderByDescending(a => a.Date)
                    .ToListAsync();

                Console.WriteLine($"📊 Found {attendances.Count} records:");
                foreach (var att in attendances)
                {
                    Console.WriteLine($"   - Date: {att.Date:yyyy-MM-dd}, Status: {att.Status}, Student: {att.Student?.FirstName}");
                }

                return new GetByIdResponseDto<List<Attendance>>
                {
                    Success = true,
                    Message = "تم جلب سجلات الحضور",
                    Data = attendances
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");

                return new GetByIdResponseDto<List<Attendance>>
                {
                    Success = false,
                    Message = $"حدث خطأ: {ex.Message}",
                    Data = new List<Attendance>()
                };
            }
        }

        public async Task<GetByIdResponseDto<object>> GetAttendanceStatsAsync(AttendanceReportDto report)
        {
            try
            {
                var query = _ctx.TbAttendance.AsQueryable();

                if (report.GroupId.HasValue)
                    query = query.Where(a => a.GroupId == report.GroupId.Value);

                if (!string.IsNullOrEmpty(report.StudentId))
                    query = query.Where(a => a.StudentId == report.StudentId);

                // ✅ استخدم Date فقط بدون الوقت
                if (report.StartDate.HasValue)
                    query = query.Where(a => a.Date.Date >= report.StartDate.Value.Date);

                if (report.EndDate.HasValue)
                    query = query.Where(a => a.Date.Date <= report.EndDate.Value.Date);

                var total = await query.CountAsync();
                var present = await query.CountAsync(a => a.Status == AttendanceStatus.Present);
                var absent = await query.CountAsync(a => a.Status == AttendanceStatus.Absent);
                var late = await query.CountAsync(a => a.Status == AttendanceStatus.Late);

                var stats = new
                {
                    Total = total,
                    Present = present,
                    Absent = absent,
                    Late = late,
                    PresentPercentage = total > 0 ? Math.Round((double)present / total * 100, 2) : 0,
                    AbsentPercentage = total > 0 ? Math.Round((double)absent / total * 100, 2) : 0,
                    LatePercentage = total > 0 ? Math.Round((double)late / total * 100, 2) : 0
                };

                return new GetByIdResponseDto<object>
                {
                    Success = true,
                    Message = "تم جلب الإحصائيات",
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<object>
                {
                    Success = false,
                    Message = $"حدث خطأ: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
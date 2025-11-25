using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class GroupRepo : IGroup
    {
        private readonly EducationalSystemApiContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupRepo(EducationalSystemApiContext ctx, UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task<GetByIdResponseDto<List<ApplicationUser>>> AddStudentToGroupAsync(string groupId, List<string> studentsIds)
        {
            try
            {
                var group = await _ctx.TbGroups
                    .Include(g => g.UserGroups)
                    .FirstOrDefaultAsync(g => g.Id.ToString() == groupId);

                if (group == null)
                    return new GetByIdResponseDto<List<ApplicationUser>> { Success = false, Message = "المجموعة غير موجودة" };

                var currentCount = group.UserGroups.Count;
                var newStudentsCount = studentsIds.Count;

                if (currentCount + newStudentsCount > group.Capacity)
                    return new GetByIdResponseDto<List<ApplicationUser>>
                    {
                        Success = false,
                        Message = $"المجموعة ممتلئة. السعة المتاحة: {group.Capacity - currentCount} والطلاب المطلوب إضافتهم: {newStudentsCount}"
                    };

                var students = await _userManager.Users
                    .Include(u => u.UserGroups)
                        .ThenInclude(ug => ug.Group)
                    .Where(s => studentsIds.Contains(s.Id))
                    .ToListAsync();

                var addedStudents = new List<ApplicationUser>();

                foreach (var student in students)
                {
                    var alreadyInGroup = student.UserGroups.Any(ug => ug.GroupId.ToString() == groupId);

                    if (alreadyInGroup)
                        return new GetByIdResponseDto<List<ApplicationUser>>
                        {
                            Success = false,
                            Message = $"الطالب/ة {student.FirstName} {student.LastName} موجود/ة في المجموعة {group.Name} بالفعل"
                        };

                    var conflictingGroup = await CheckTimeConflictAsync(student, group);

                    if (conflictingGroup != null)
                    {
                        var conflictEndTime = conflictingGroup.StartTime.Add(TimeSpan.FromHours(conflictingGroup.DurationInHours));

                        return new GetByIdResponseDto<List<ApplicationUser>>
                        {
                            Success = false,
                            Message = $"الطالب/ة {student.FirstName} {student.LastName} مسجل/ة في مجموعة '{conflictingGroup.Name}' " +
                                     $"يوم {GetArabicDayName(conflictingGroup.DayOfWeek)} " +
                                     $"من الساعة {conflictingGroup.StartTime:hh\\:mm} إلى {conflictEndTime:hh\\:mm}. " +
                                     $"يوجد تعارض مع المجموعة المطلوبة."
                        };
                    }

                    var userGroup = new UserGroup
                    {
                        UserId = student.Id,
                        GroupId = group.Id,
                        JoinedDate = DateTime.UtcNow
                    };

                    await _ctx.TbUserGroups.AddAsync(userGroup);
                    addedStudents.Add(student);
                }

                await _ctx.SaveChangesAsync();

                return new GetByIdResponseDto<List<ApplicationUser>>
                {
                    Success = true,
                    Message = "تم إضافة الطلاب إلى المجموعة بنجاح",
                    Data = addedStudents
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<List<ApplicationUser>>
                {
                    Success = false,
                    Message = $"حدثت مشكلة أثناء إضافة الطلاب للمجموعة: {ex.Message}"
                };
            }
        }

        private async Task<Group?> CheckTimeConflictAsync(ApplicationUser student, Group targetGroup)
        {
            var studentGroups = student.UserGroups
                .Where(ug => ug.Group != null)
                .Select(ug => ug.Group)
                .ToList();

            var targetEndTime = targetGroup.StartTime.Add(TimeSpan.FromHours(targetGroup.DurationInHours));

            foreach (var group in studentGroups)
            {
                if (group.Id == targetGroup.Id)
                    continue;

                if (group.DayOfWeek != targetGroup.DayOfWeek)
                    continue;

                var groupEndTime = group.StartTime.Add(TimeSpan.FromHours(group.DurationInHours));

                bool hasOverlap = group.StartTime < targetEndTime && targetGroup.StartTime < groupEndTime;

                if (hasOverlap)
                    return group;
            }

            return null;
        }

        private string GetArabicDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Saturday => "السبت",
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الاثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                _ => day.ToString()
            };
        }

        public async Task<ResponseMessage> CreateNewGroupAsync(Group group)
        {
            if (group == null)
                return new ResponseMessage { success = false, message = "البيانات المرسلة غير صحيحة" };

            var existGroup = await _ctx.TbGroups
                .FirstOrDefaultAsync(g => g.Name == group.Name);

            if (existGroup != null)
                return new ResponseMessage { success = false, message = "تم إنشاء المجموعة من قبل" };

            try
            {
                await _ctx.TbGroups.AddAsync(group);
                await _ctx.SaveChangesAsync();
                return new ResponseMessage { success = true, message = "تم إنشاء المجموعة بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = $"حدث مشكلة أثناء إضافة المجموعة: {ex.Message}" };
            }
        }

        public async Task<ResponseMessage> DeleteGroupAsync(string id)
        {
            var group = await _ctx.TbGroups
                .Include(g => g.UserGroups)
                .FirstOrDefaultAsync(g => g.Id.ToString() == id);

            if (group == null)
                return new ResponseMessage { success = false, message = "المجموعة غير موجودة" };

            try
            {
                // UserGroups will be deleted automatically by cascade delete
                _ctx.TbGroups.Remove(group);
                await _ctx.SaveChangesAsync();
                return new ResponseMessage { success = true, message = "تم إزالة المجموعة بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = $"حدثت مشكلة أثناء إزالة المجموعة: {ex.Message}" };
            }
        }

        public async Task<ResponseMessage> DeleteStudentFromGroupAsync(string groupId, string studentId)
        {
            try
            {
                // Find the UserGroup relationship
                var userGroup = await _ctx.TbUserGroups
                    .FirstOrDefaultAsync(ug => ug.UserId == studentId && ug.GroupId.ToString() == groupId);

                if (userGroup == null)
                    return new ResponseMessage { success = false, message = "الطالب غير موجود في هذه المجموعة" };

                // Remove the relationship
                _ctx.TbUserGroups.Remove(userGroup);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم إزالة الطالب من المجموعة بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = $"حدثت مشكلة أثناء إزالة الطالب من المجموعة: {ex.Message}" };
            }
        }

        public async Task<GetByIdResponseDto<List<Group>>> GetAllGroupsAsync()
        {
            try
            {
                var groups = await _ctx.TbGroups
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .AsNoTracking()
                    .ToListAsync();

                return new GetByIdResponseDto<List<Group>>
                {
                    Success = true,
                    Message = "تم جلب كل المجموعات",
                    Data = groups
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<List<Group>>
                {
                    Success = false,
                    Message = $"حدثت مشكلة أثناء جلب المجموعات: {ex.Message}"
                };
            }
        }

        public async Task<GetByIdResponseDto<Group>> GetGroupByIdAsync(string id)
        {
            try
            {
                var group = await _ctx.TbGroups
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .FirstOrDefaultAsync(g => g.Id.ToString() == id);

                if (group == null)
                    return new GetByIdResponseDto<Group>
                    {
                        Success = false,
                        Message = "المجموعة غير موجودة",
                        Data = null
                    };

                return new GetByIdResponseDto<Group>
                {
                    Success = true,
                    Message = "تم جلب المجموعة",
                    Data = group
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<Group>
                {
                    Success = false,
                    Message = $"حدثت مشكلة أثناء جلب المجموعة: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GetByIdResponseDto<List<ApplicationUser>>> GetStudentsByGroupIdAsync(string id)
        {
            try
            {
                var group = await _ctx.TbGroups
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .FirstOrDefaultAsync(g => g.Id.ToString() == id);

                if (group == null)
                {
                    return new GetByIdResponseDto<List<ApplicationUser>>
                    {
                        Success = false,
                        Message = "المجموعة غير موجودة",
                        Data = new List<ApplicationUser>()
                    };
                }

                // Extract users from UserGroups
                var students = group.UserGroups
                    .Select(ug => ug.User)
                    .Where(u => u != null)
                    .ToList();

                return new GetByIdResponseDto<List<ApplicationUser>>
                {
                    Success = true,
                    Message = "تم جلب الطلاب بنجاح",
                    Data = students
                };
            }
            catch (Exception ex)
            {
                return new GetByIdResponseDto<List<ApplicationUser>>
                {
                    Success = false,
                    Message = $"حدثت مشكلة أثناء جلب طلاب المجموعة: {ex.Message}",
                    Data = new List<ApplicationUser>()
                };
            }
        }

        public async Task<ResponseMessage> UpdateGroupAsync(UpdateGroupDto request)
        {
            if (request == null || request.Id == Guid.Empty)
                return new ResponseMessage { success = false, message = "بيانات المجموعة غير صحيحة" };

            try
            {
                var existingGroup = await _ctx.TbGroups
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                            .ThenInclude(u => u.UserGroups)
                                .ThenInclude(ug => ug.Group)
                    .FirstOrDefaultAsync(g => g.Id == request.Id);

                if (existingGroup == null)
                    return new ResponseMessage { success = false, message = "المجموعة غير موجودة" };

                // Check if new capacity is less than current students count
                var currentStudentsCount = existingGroup.UserGroups.Count;
                if (request.Capacity < currentStudentsCount)
                    return new ResponseMessage
                    {
                        success = false,
                        message = $"لا يمكن تقليل السعة إلى {request.Capacity}. العدد الحالي للطلاب: {currentStudentsCount}"
                    };

                // ========== فحص التعارض الزمني عند تعديل الوقت أو اليوم ==========
                bool timeChanged = existingGroup.DayOfWeek != request.DayOfWeek ||
                                  existingGroup.StartTime != request.StartTime ||
                                  existingGroup.DurationInHours != request.DurationInHours;

                if (timeChanged)
                {
                    // إنشاء مجموعة مؤقتة بالبيانات الجديدة للفحص
                    var tempGroup = new Group
                    {
                        Id = existingGroup.Id,
                        DayOfWeek = request.DayOfWeek,
                        StartTime = request.StartTime,
                        DurationInHours = request.DurationInHours
                    };

                    // فحص كل الطلاب في المجموعة
                    foreach (var userGroup in existingGroup.UserGroups)
                    {
                        var student = userGroup.User;
                        var conflictingGroup = await CheckTimeConflictAsync(student, tempGroup);

                        if (conflictingGroup != null)
                        {
                            var conflictEndTime = conflictingGroup.StartTime.Add(TimeSpan.FromHours(conflictingGroup.DurationInHours));

                            return new ResponseMessage
                            {
                                success = false,
                                message = $"لا يمكن تعديل موعد المجموعة. الطالب/ة {student.FirstName} {student.LastName} " +
                                         $"مسجل/ة في مجموعة '{conflictingGroup.Name}' " +
                                         $"يوم {GetArabicDayName(conflictingGroup.DayOfWeek)} " +
                                         $"من الساعة {conflictingGroup.StartTime:hh\\:mm} إلى {conflictEndTime:hh\\:mm}"
                            };
                        }
                    }
                }

                // تحديث البيانات
                existingGroup.Name = request.Name;
                existingGroup.Subject = request.Subject;
                existingGroup.InstructorName = request.InstructorName;
                existingGroup.AssistantId = request.AssistantId;
                existingGroup.Capacity = request.Capacity;
                existingGroup.StartDate = request.StartDate;
                existingGroup.FeesPerLesson = request.FeesPerLesson;
                existingGroup.IsActive = request.IsActive;
                existingGroup.DayOfWeek = request.DayOfWeek;
                existingGroup.StartTime = request.StartTime;
                existingGroup.DurationInHours = request.DurationInHours;
                existingGroup.Location = request.Location;

                _ctx.TbGroups.Update(existingGroup);
                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تحديث بيانات المجموعة بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = $"حدث خطأ أثناء تحديث بيانات المجموعة: {ex.Message}" };
            }
        }
    }
}
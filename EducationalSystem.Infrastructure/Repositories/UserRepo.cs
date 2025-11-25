// UserRepo.cs
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class UserRepo : IUser
    {
        private readonly EducationalSystemApiContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepo(EducationalSystemApiContext ctx, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessage> CreateNewUserAsync(CreateUserDto user)
        {
            var existEmail = await _userManager.FindByEmailAsync(user.Email);
            if (existEmail != null)
                return new ResponseMessage { success = false, message = "الحساب موجود بالفعل" };

            // Validate groups (capacity and time conflicts)
            if (user.GroupIds != null && user.GroupIds.Any())
            {
                var validationResult = await ValidateGroupsForUser(null, user.GroupIds);
                if (!validationResult.success)
                    return validationResult;
            }

            var appUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);

            if (!result.Succeeded)
                return new ResponseMessage { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            await _userManager.AddToRoleAsync(appUser, user.Role);

            // Add user to groups
            if (user.GroupIds != null && user.GroupIds.Any())
            {
                foreach (var groupId in user.GroupIds)
                {
                    var userGroup = new UserGroup
                    {
                        UserId = appUser.Id,
                        GroupId = groupId,
                        JoinedDate = DateTime.UtcNow
                    };
                    await _ctx.TbUserGroups.AddAsync(userGroup);
                }
                await _ctx.SaveChangesAsync();
            }

            return new ResponseMessage { success = true, message = "تم إنشاء الحساب بنجاح" };
        }

        public async Task<ResponseMessage> UpdateUserAsync(CreateUserDto user)
        {
            if (user == null || user.Id == Guid.Empty.ToString())
                return new ResponseMessage { success = false, message = "بيانات المستخدم غير صحيحة" };

            try
            {
                var existingUser = await _userManager.Users
                    .Include(u => u.UserGroups)
                        .ThenInclude(ug => ug.Group)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (existingUser == null)
                    return new ResponseMessage { success = false, message = "المستخدم غير موجود" };

                // Validate groups (capacity and time conflicts)
                if (user.GroupIds != null && user.GroupIds.Any())
                {
                    var validationResult = await ValidateGroupsForUser(user.Id, user.GroupIds);
                    if (!validationResult.success)
                        return validationResult;
                }

                // Update user basic info
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.UserName = user.Email;
                existingUser.Role = user.Role;
                existingUser.PhoneNumber = user.PhoneNumber;

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                    return new ResponseMessage { success = false, message = "حدث خطأ أثناء تحديث بيانات المستخدم" };

                // Update password if provided
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var passResult = await _userManager.ResetPasswordAsync(existingUser, token, user.Password);

                    if (!passResult.Succeeded)
                        return new ResponseMessage { success = false, message = "حدث خطأ أثناء تحديث كلمة المرور" };
                }

                // Update groups
                var existingUserGroups = await _ctx.TbUserGroups
                    .Where(ug => ug.UserId == user.Id)
                    .ToListAsync();
                _ctx.TbUserGroups.RemoveRange(existingUserGroups);

                // Add new group associations
                if (user.GroupIds != null && user.GroupIds.Any())
                {
                    foreach (var groupId in user.GroupIds)
                    {
                        var userGroup = new UserGroup
                        {
                            UserId = user.Id,
                            GroupId = groupId,
                            JoinedDate = DateTime.UtcNow
                        };
                        await _ctx.TbUserGroups.AddAsync(userGroup);
                    }
                }

                await _ctx.SaveChangesAsync();

                return new ResponseMessage { success = true, message = "تم تحديث بيانات المستخدم بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = $"حدث خطأ أثناء تحديث بيانات المستخدم: {ex.Message}" };
            }
        }

        // ========== دالة التحقق من التعارض والسعة ==========
        private async Task<ResponseMessage> ValidateGroupsForUser(string? userId, List<Guid> groupIds)
        {
            var groupsToCheck = await _ctx.TbGroups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            if (groupsToCheck.Count != groupIds.Count)
                return new ResponseMessage { success = false, message = "بعض المجموعات غير موجودة" };

            // فحص السعة
            foreach (var group in groupsToCheck)
            {
                var currentCount = await _ctx.TbUserGroups.CountAsync(ug => ug.GroupId == group.Id);

                // إذا كان تحديث، لا نحسب المستخدم الحالي
                if (!string.IsNullOrEmpty(userId))
                {
                    var userAlreadyInGroup = await _ctx.TbUserGroups
                        .AnyAsync(ug => ug.UserId == userId && ug.GroupId == group.Id);

                    if (userAlreadyInGroup)
                        currentCount--; // نقلل العدد لأن المستخدم موجود بالفعل
                }

                if (currentCount >= group.Capacity)
                    return new ResponseMessage { success = false, message = $"المجموعة '{group.Name}' ممتلئة" };
            }

            // فحص التعارض الزمني
            for (int i = 0; i < groupsToCheck.Count; i++)
            {
                for (int j = i + 1; j < groupsToCheck.Count; j++)
                {
                    var group1 = groupsToCheck[i];
                    var group2 = groupsToCheck[j];

                    // تحقق من تطابق اليوم
                    if (group1.DayOfWeek == group2.DayOfWeek)
                    {
                        // تحقق من تداخل الأوقات
                        var end1 = group1.StartTime.Add(TimeSpan.FromHours(group1.DurationInHours));
                        var end2 = group2.StartTime.Add(TimeSpan.FromHours(group2.DurationInHours));

                        bool hasOverlap = group1.StartTime < end2 && group2.StartTime < end1;

                        if (hasOverlap)
                        {
                            return new ResponseMessage
                            {
                                success = false,
                                message = $"يوجد تعارض زمني بين المجموعتين '{group1.Name}' و '{group2.Name}' يوم {GetArabicDayName(group1.DayOfWeek)}"
                            };
                        }
                    }
                }
            }

            // إذا كان هناك userId (تحديث)، تحقق من التعارض مع المجموعات الموجودة
            if (!string.IsNullOrEmpty(userId))
            {
                var existingUserGroups = await _ctx.TbUserGroups
                    .Where(ug => ug.UserId == userId && !groupIds.Contains(ug.GroupId))
                    .Include(ug => ug.Group)
                    .Select(ug => ug.Group)
                    .ToListAsync();

                foreach (var existingGroup in existingUserGroups)
                {
                    foreach (var newGroup in groupsToCheck)
                    {
                        // تحقق من تطابق اليوم
                        if (existingGroup.DayOfWeek == newGroup.DayOfWeek)
                        {
                            // تحقق من تداخل الأوقات
                            var endExisting = existingGroup.StartTime.Add(TimeSpan.FromHours(existingGroup.DurationInHours));
                            var endNew = newGroup.StartTime.Add(TimeSpan.FromHours(newGroup.DurationInHours));

                            bool hasOverlap = existingGroup.StartTime < endNew && newGroup.StartTime < endExisting;

                            if (hasOverlap)
                            {
                                return new ResponseMessage
                                {
                                    success = false,
                                    message = $"المستخدم مسجل بالفعل في مجموعة '{existingGroup.Name}' يوم {GetArabicDayName(existingGroup.DayOfWeek)} من الساعة {existingGroup.StartTime:hh\\:mm} إلى {endExisting:hh\\:mm}. يوجد تعارض مع المجموعة '{newGroup.Name}'"
                                };
                            }
                        }
                    }
                }
            }

            return new ResponseMessage { success = true };
        }

        // ========== دالة تحويل اليوم للعربي ==========
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

        public async Task<ResponseMessage> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ResponseMessage { success = false, message = "المستخدم غير موجود" };

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return new ResponseMessage
                    {
                        success = false,
                        message = "حدث خطأ أثناء إزالة المستخدم"
                    };

                return new ResponseMessage { success = true, message = "تم إزالة المستخدم بنجاح" };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { success = false, message = "حدثت مشكلة أثناء إزالة المستخدم" };
            }
        }

        public async Task<PaginatedResponseDto<List<ApplicationUser>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _userManager.Users.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var allUsers = await _userManager.Users
                    .Include(u => u.UserGroups)
                        .ThenInclude(ug => ug.Group)
                    .AsNoTracking()
                    .ToListAsync();

                var users = await _userManager.Users
                    .Include(u => u.UserGroups)
                        .ThenInclude(ug => ug.Group)
                    .AsNoTracking()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedResponseDto<List<ApplicationUser>>
                {
                    Success = true,
                    Message = "تم جلب جميع المستخدمين",
                    Data = users,
                    AllUsers = allUsers,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResponseDto<List<ApplicationUser>>
                {
                    Success = false,
                    Message = "حدثت مشكلة أثناء جلب المستخدمين",
                    Data = new List<ApplicationUser>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = 0
                };
            }
        }

        public async Task<GetByIdResponseDto<ApplicationUser>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return new GetByIdResponseDto<ApplicationUser>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };

            return new GetByIdResponseDto<ApplicationUser>
            {
                Success = true,
                Message = $"أهلا بك يا {user.FirstName} في صفحتك الخاصة",
                Data = user
            };
        }
    }
}
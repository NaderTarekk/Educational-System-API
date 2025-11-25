using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EducationalSystem.Infrastructure.Repositories
{
    public class AuthRepo : IAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwt;

        public AuthRepo(IJwtService jwt, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwt = jwt;
        }

        public async Task<ResponseMessage> Login(LoginDto request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
            if (!result.Succeeded)
                return new ResponseMessage { success = false, message = "خطأ في الإيميل او الباسورد" };

            var user = await _userManager.FindByEmailAsync(request.Email);
            var token = _jwt.CreateToken(user);
            return new ResponseMessage { success = true, message = "تم تسجيل الدخول", token = token, role = user.Role, user = user };
        }

        public async Task<string> RefreshTokenAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var token = _jwt.CreateToken(user);
            return token;
        }

        public async Task<ResponseMessage> Register(RegisterDto request)
        {
            var existEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existEmail != null)
                return new ResponseMessage { success = true, message = "الحساب موجود بالفعل" };
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Role = request.Role
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new ResponseMessage { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            if (await _roleManager.RoleExistsAsync("Student"))
                await _userManager.AddToRoleAsync(user, "Student");

            var getUser = await _userManager.FindByEmailAsync(request.Email);
            var token = _jwt.CreateToken(getUser);

            return new ResponseMessage { success = true, message = "تم إنشاء الحساب بنجاح", token = token };
        }
    }
}

using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IAuth
    {
        Task<ResponseMessage> Register(RegisterDto request);
        Task<ResponseMessage> Login(LoginDto request);
        Task<string> RefreshTokenAsync(string id);
    }
}

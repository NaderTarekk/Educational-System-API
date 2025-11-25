using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IUser
    {
        Task<PaginatedResponseDto<List<ApplicationUser>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
        Task<ResponseMessage> CreateNewUserAsync(CreateUserDto user);
        Task<GetByIdResponseDto<ApplicationUser>> GetUserByIdAsync(string id);
        Task<ResponseMessage> UpdateUserAsync(CreateUserDto user);
        Task<ResponseMessage> DeleteUserAsync(string id);
    }
}

using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IGroup
    {
        Task<GetByIdResponseDto<List<Group>>> GetAllGroupsAsync();
        Task<ResponseMessage> CreateNewGroupAsync(Group group);
        Task<GetByIdResponseDto<Group>> GetGroupByIdAsync(string id);
        Task<GetByIdResponseDto<List<ApplicationUser>>> GetStudentsByGroupIdAsync(string id);
        Task<ResponseMessage> UpdateGroupAsync(UpdateGroupDto request);
        Task<ResponseMessage> DeleteGroupAsync(string id);
        Task<ResponseMessage> DeleteStudentFromGroupAsync(string groupId, string studentId);
        Task<GetByIdResponseDto<List<ApplicationUser>>> AddStudentToGroupAsync(string groupId, List<string> studentsIds);
    }
}

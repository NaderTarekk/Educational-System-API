using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.AddStudentsToGroup
{
    public record AddStudentsToGroupCommand(string groupId, List<string> studentsIds) : IRequest<GetByIdResponseDto<List<ApplicationUser>>>;
}

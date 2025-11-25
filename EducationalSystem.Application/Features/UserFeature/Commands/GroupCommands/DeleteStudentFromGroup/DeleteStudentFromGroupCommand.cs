using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteStudentFromGroup
{
    public record DeleteStudentFromGroupCommand(string groupId, string studentId):IRequest<ResponseMessage>;
}

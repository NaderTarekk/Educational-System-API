using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.UpdateGroup
{
    public record UpdateGroupCommand(UpdateGroupDto group) : IRequest<ResponseMessage>;
}

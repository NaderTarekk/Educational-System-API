using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.CreateNewGroup
{
    public record CreateNewGroupCommand(Group group) : IRequest<ResponseMessage>;
}

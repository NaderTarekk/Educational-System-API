using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteGroup
{
    public record DeleteGroupCommand(string id): IRequest<ResponseMessage>;
}

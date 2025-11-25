using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.DeleteUser
{
    public record DeleteUserCommand(string id) : IRequest<ResponseMessage>;

}

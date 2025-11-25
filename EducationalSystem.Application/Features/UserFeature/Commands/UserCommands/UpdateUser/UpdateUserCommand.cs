using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.UpdateUser
{
    public record UpdateteUserCommand(CreateUserDto user) : IRequest<ResponseMessage>;

}

using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.CreateUser
{
    public record CreateUserCommand(CreateUserDto user):IRequest<ResponseMessage>;
}

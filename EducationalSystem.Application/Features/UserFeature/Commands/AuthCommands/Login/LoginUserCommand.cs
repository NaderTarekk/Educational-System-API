using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.AuthCommands.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<object>;
}

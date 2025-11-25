using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.AuthCommands.Register
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Role,
        int Age
    ) : IRequest<object>;
}

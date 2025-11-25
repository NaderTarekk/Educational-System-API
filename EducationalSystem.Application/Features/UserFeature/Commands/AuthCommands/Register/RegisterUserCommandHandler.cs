using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.AuthCommands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, object>
    {
        private readonly IAuth _user;
        public RegisterUserCommandHandler(IAuth user)
        {
            _user = user;
        }
        public async Task<object> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var mappingUser = new RegisterDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = request.Role,
                Age = request.Age,
            };
            return await _user.Register(mappingUser);
        }
    }
}

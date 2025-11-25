using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.AuthCommands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, object>
    {
        private readonly IAuth _user;
        public LoginUserCommandHandler(IAuth user)
        {
            _user = user;
        }
        public async Task<object> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var mappingUser = new LoginDto
            {
                Email = request.Email,
                Password = request.Password
            };
            return await _user.Login(mappingUser);
        }
    }
}

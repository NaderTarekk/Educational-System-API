using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseMessage>
    {
        private readonly IUser _user;
     
        public CreateUserCommandHandler(IUser user)
        {
            _user = user;
        }

        public async Task<ResponseMessage> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _user.CreateNewUserAsync(request.user);
        }
    }
}

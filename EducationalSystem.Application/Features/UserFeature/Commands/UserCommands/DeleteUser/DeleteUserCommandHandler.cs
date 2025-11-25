using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseMessage>
    {
        private readonly IUser _user;

        public DeleteUserCommandHandler(IUser user)
        {
            _user = user;
        }
        public async Task<ResponseMessage> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _user.DeleteUserAsync(request.id);
        }
    }
}

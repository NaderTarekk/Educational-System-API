using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.UserCommands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateteUserCommand, ResponseMessage>
    {
        private readonly IUser _user;

        public UpdateUserCommandHandler(IUser user)
        {
            _user = user;
        }
        public async Task<ResponseMessage> Handle(UpdateteUserCommand request, CancellationToken cancellationToken)
        {
            return await _user.UpdateUserAsync(request.user);
        }
    }
}

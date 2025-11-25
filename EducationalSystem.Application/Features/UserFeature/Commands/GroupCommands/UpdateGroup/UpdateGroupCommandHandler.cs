using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.UpdateGroup
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, ResponseMessage>
    {
        private readonly IGroup _group;
        public UpdateGroupCommandHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<ResponseMessage> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            return await _group.UpdateGroupAsync(request.group);
        }
    }
}

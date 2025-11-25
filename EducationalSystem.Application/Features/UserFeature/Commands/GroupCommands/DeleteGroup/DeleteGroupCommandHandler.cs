using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, ResponseMessage>
    {
        private readonly IGroup _group;
        public DeleteGroupCommandHandler(IGroup group)
        {
            _group = group;
        }
        public Task<ResponseMessage> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            return _group.DeleteGroupAsync(request.id);
        }
    }
}

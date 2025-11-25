using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.CreateNewGroup
{
    public class CreateNewGroupCommandHandler : IRequestHandler<CreateNewGroupCommand, ResponseMessage>
    {
        private readonly IGroup _group;
        public CreateNewGroupCommandHandler(IGroup group)
        {
            _group = group;
        }
        public Task<ResponseMessage> Handle(CreateNewGroupCommand request, CancellationToken cancellationToken)
        {
            return _group.CreateNewGroupAsync(request.group);
        }
    }
}

using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.DeleteStudentFromGroup
{
    public class DeleteStudentFromGroupCommandHandler : IRequestHandler<DeleteStudentFromGroupCommand, ResponseMessage>
    {
        private readonly IGroup _group;
        public DeleteStudentFromGroupCommandHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<ResponseMessage> Handle(DeleteStudentFromGroupCommand request, CancellationToken cancellationToken)
        {
            return await _group.DeleteStudentFromGroupAsync(request.groupId, request.studentId);
        }
    }
}

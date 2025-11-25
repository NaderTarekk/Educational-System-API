using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Commands.GroupCommands.AddStudentsToGroup
{
    public class AddStudentsToGroupCommandHandler : IRequestHandler<AddStudentsToGroupCommand, GetByIdResponseDto<List<ApplicationUser>>>
    {
        private readonly IGroup _group;
        public AddStudentsToGroupCommandHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<GetByIdResponseDto<List<ApplicationUser>>> Handle(AddStudentsToGroupCommand request, CancellationToken cancellationToken)
        {
            return await _group.AddStudentToGroupAsync(request.groupId, request.studentsIds);
        }
    }
}

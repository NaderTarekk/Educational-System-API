using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetStudentsByGroupId
{
    public class GetStudentsByGroupIdQueryHandler : IRequestHandler<GetStudentsByGroupIdQuery, GetByIdResponseDto<List<ApplicationUser>>>
    {
        private readonly IGroup _group;
        public GetStudentsByGroupIdQueryHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<GetByIdResponseDto<List<ApplicationUser>>> Handle(GetStudentsByGroupIdQuery request, CancellationToken cancellationToken)
        {
            return await _group.GetStudentsByGroupIdAsync(request.id);
        }
    }
}

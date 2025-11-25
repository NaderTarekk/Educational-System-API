using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetAllGroups
{
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, GetByIdResponseDto<List<Group>>>
    {
        private readonly IGroup _group;
        public GetAllGroupQueryHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<GetByIdResponseDto<List<Group>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            return await _group.GetAllGroupsAsync();
        }
    }
}

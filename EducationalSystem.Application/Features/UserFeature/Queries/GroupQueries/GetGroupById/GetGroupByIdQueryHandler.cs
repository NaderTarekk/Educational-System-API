using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetGroupById
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GetByIdResponseDto<Group>>
    {
        private readonly IGroup _group;
        public GetGroupByIdQueryHandler(IGroup group)
        {
            _group = group;
        }
        public async Task<GetByIdResponseDto<Group>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            return await _group.GetGroupByIdAsync(request.id);
        }
    }
}

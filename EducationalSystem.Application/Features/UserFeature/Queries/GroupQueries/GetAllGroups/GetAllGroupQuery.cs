using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetAllGroups
{
    public record GetAllGroupQuery : IRequest<GetByIdResponseDto<List<Group>>>;
}

using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetGroupById
{
    public record GetGroupByIdQuery(string id) : IRequest<GetByIdResponseDto<Group>>;
}

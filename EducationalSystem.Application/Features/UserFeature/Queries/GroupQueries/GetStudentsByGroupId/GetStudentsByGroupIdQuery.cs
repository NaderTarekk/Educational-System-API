using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.GroupQueries.GetStudentsByGroupId
{
    public record GetStudentsByGroupIdQuery(string id):IRequest<GetByIdResponseDto<List<ApplicationUser>>>;
}

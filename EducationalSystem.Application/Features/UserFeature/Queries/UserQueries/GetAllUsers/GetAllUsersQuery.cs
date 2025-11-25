using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.UserQueries.GetAllUsers
{
    public record GetAllUsersQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PaginatedResponseDto<List<ApplicationUser>>>;
}

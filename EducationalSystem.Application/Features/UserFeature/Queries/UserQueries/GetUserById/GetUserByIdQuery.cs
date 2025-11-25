using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.UserQueries.GetUserById
{
    public record GetUserByIdQuery(string id) : IRequest<GetByIdResponseDto<ApplicationUser>>;
}

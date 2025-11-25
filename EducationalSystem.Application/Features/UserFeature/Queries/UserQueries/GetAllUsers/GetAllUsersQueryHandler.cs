using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Entities;
using MediatR;
using EducationalSystem.Domain.Interfaces;

namespace EducationalSystem.Application.Features.User.Queries.UserQueries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedResponseDto<List<ApplicationUser>>>
    {
        private readonly IUser _user;

        public GetAllUsersQueryHandler(IUser user)
        {
            _user = user;
        }
        public async Task<PaginatedResponseDto<List<ApplicationUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _user.GetAllUsersAsync(request.pageNumber, request.pageSize);
        }
    }
}

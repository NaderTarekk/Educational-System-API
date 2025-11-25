using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Entities.DTOs;
using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.UserQueries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetByIdResponseDto<ApplicationUser>>
    {
        private readonly IUser _user;

        public GetUserByIdQueryHandler(IUser user)
        {
            _user = user;
        }
        public async Task<GetByIdResponseDto<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _user.GetUserByIdAsync(request.id);
        }
    }
}

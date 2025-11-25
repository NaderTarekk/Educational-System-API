using EducationalSystem.Domain.Interfaces;
using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.Auth.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, string>
    {
        private readonly IAuth _user;
        public RefreshTokenQueryHandler(IAuth user)
        {
            _user = user;
        }
        public async Task<string> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return await _user.RefreshTokenAsync(request.id);
        }
    }
}

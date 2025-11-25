using MediatR;

namespace EducationalSystem.Application.Features.User.Queries.Auth.RefreshToken
{
    public record RefreshTokenQuery(string id) : IRequest<string>;
}

using EducationalSystem.Domain.Entities;

namespace EducationalSystem.Domain.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(ApplicationUser user);
    }
}

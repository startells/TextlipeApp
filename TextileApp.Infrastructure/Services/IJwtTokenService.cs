using TextileApp.Domain.Entities;

namespace TextileApp.Infrastructure.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user, List<string?>? roles = null);
}
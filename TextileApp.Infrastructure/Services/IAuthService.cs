using TextileApp.Contracts.DTO.Request;
using TextileApp.Contracts.DTO.Response;

namespace TextileApp.Infrastructure.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
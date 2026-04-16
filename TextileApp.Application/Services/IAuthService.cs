using TextileApp.Contracts.DTO.Request;
using TextileApp.Contracts.DTO.Response;

namespace TextileApp.Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
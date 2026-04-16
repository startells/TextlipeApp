using System.Security.Cryptography;
using System.Text;
using TextileApp.Application.Services;
using TextileApp.Contracts.DTO.Request;
using TextileApp.Contracts.DTO.Response;
using TextileApp.Infrastructure.Data;

namespace TextileApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Username and password are required"
            };
        }
        
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

        if (user == null || !user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }
        
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }

        return new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Token = GenerateToken(user),
            UserId = user.Id
        };
    }
    
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }

    private string GenerateToken(Domain.Entities.User user)
    {
        // TODO: Добавить JWT позже
        return Guid.NewGuid().ToString();
    }
}
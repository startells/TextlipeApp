using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using TextileApp.Contracts.DTO.Request;
using TextileApp.Contracts.DTO.Response;
using TextileApp.Infrastructure.Data;

namespace TextileApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    
    public AuthService(IUnitOfWork unitOfWork, ILogger<AuthService> logger, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with empty credentials");
            throw new ArgumentException("Username and password cannot be empty");
        }
        
        // Берём user по Логину с ролями
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

        if (user == null || !user.IsActive)
        {
            _logger.LogWarning($"Login attempt for non-existent or inactive user: {request.Username}");
            throw new UnauthorizedAccessException("Invalid username or password");
        }
        
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning($"Failed login attempt for user: {request.Username}");
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        _logger.LogInformation($"Successful login for user: {request.Username}");
        
        return new LoginResponse
        (
            GenerateToken(user),
            user.Id
        );
    }
    
    /// <summary>
    /// Хеширует пароль с использованием Bcrypt
    /// </summary>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));
            
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    /// <summary>
    /// Проверяет пароль против хеша
    /// </summary>
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (SaltParseException)
        {
            _logger.LogError("Invalid password hash format");
            return false;
        }
    }

    private string GenerateToken(Domain.Entities.User user)
    {
        // Получаем роли пользователя
        var roles = user.UserRoles?.Select(ur => ur.Role?.Name).Where(r => r != null).ToList();
        return _jwtTokenService.GenerateToken(user, roles);
    }
}
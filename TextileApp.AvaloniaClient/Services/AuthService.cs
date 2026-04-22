using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TextileApp.AvaloniaClient.Services;

public class AuthService
{
    private readonly ApiService _apiService;
    private readonly SessionService _sessionService;
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(ApiService apiService, SessionService sessionService, ILogger<AuthService> logger)
    {
        _apiService = apiService;
        _sessionService = sessionService;
        _logger = logger;
    }
    
    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var request = new Contracts.DTO.Request.LoginRequest
            (
                username,
                password
            );
            
            var response = await _apiService.PostAsync<Contracts.DTO.Request.LoginRequest, Contracts.DTO.Response.LoginResponse>("Auth/login", request);
            
            if (response != null)
            {
                _sessionService.SetToken(response.Token);
                _logger.LogInformation($"Login successful for user: {username}");
                return true;
            }
            
            _logger.LogWarning($"Login failed for user: {username} - no response");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Login error for user: {username}");
            throw;
        }
    }
}
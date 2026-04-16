using Microsoft.AspNetCore.Mvc;
using TextileApp.Application.Services;
using TextileApp.Contracts.DTO.Request;

namespace TextileApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }
}
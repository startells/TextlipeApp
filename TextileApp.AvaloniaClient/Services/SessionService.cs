using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace TextileApp.AvaloniaClient.Services;

public class SessionService
{
    public string? Token { get; private set; }
    public int UserId { get; private set; }
    public List<string> Roles { get; private set; } = new();

    public void SetToken(string token)
    {
        Token = token;
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        UserId = int.Parse(jwt.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        Roles = jwt.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }
    
    public bool HasRole(string role) => Roles.Contains(role);
}
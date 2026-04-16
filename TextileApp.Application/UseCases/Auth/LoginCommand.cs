namespace TextileApp.Application.UseCases.Auth;

public class LoginCommand
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
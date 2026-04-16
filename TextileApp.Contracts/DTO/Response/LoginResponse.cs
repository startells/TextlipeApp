namespace TextileApp.Contracts.DTO.Response;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string? Token { get; set; }
    public int? UserId { get; set; }
}
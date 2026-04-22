namespace TextileApp.Contracts.DTO.Request;

public record LoginRequest(
    string Username,
    string Password
    );
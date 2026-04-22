namespace TextileApp.Contracts.DTO.Response;

public record LoginResponse(
    string Token,
    int UserId
    );
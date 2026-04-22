namespace TextileApp.Contracts.DTO.Response;

public record ErrorResponse
(
    string Type,
    string Detail,
    int Status
);
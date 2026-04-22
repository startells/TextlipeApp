namespace TextileApp.Domain.Entities;

public class UserRoleHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Username { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public string Action { get; set; } = null!;
    // Assigned, Removed, Updated

    public int ChangedByUserId { get; set; }
    public string ChangedByUsername { get; set; } = null!;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public string? Reason { get; set; }
}
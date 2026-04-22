using TextileApp.Domain.Entities;

namespace TextileApp.Infrastructure.Services;

public interface IAdminService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    
}
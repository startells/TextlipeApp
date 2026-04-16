using TextileApp.Domain.Entities;

namespace TextileApp.Infrastructure.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int userId);
    Task<bool> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
}
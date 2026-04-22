using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TextileApp.Domain.Entities;

namespace TextileApp.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;
    
    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        try
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user by username: {username}");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user by id: {id}");
            throw;
        }
    }

    public async Task<bool> CreateAsync(User user)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User created successfully: {user.Username}");
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Database error creating user: {user?.Username}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error creating user: {user?.Username}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(User user)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User updated successfully: {user.Username}");
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Database error updating user: {user?.Username}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error updating user: {user?.Username}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User deleted successfully: {user.Username}");
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Database error deleting user with id: {id}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error deleting user with id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        try
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }
}

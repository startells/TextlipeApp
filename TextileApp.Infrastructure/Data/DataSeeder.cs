using Microsoft.EntityFrameworkCore;
using TextileApp.Domain.Constants;
using TextileApp.Domain.Entities;
using TextileApp.Infrastructure.Services;

namespace TextileApp.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IAuthService authService)
    {
        await SeedRoles(context);
        await SeedAdmin(context, authService);
    }

    private static async Task SeedRoles(ApplicationDbContext context)
    {
        var existingRoles = await context.Roles
            .Select(r => r.Name)
            .ToListAsync();

        var rolesToAdd = RolesConstants.All
            .Where(role => !existingRoles.Contains(role))
            .Select(role => new Role { Name = role })
            .ToList();

        if (rolesToAdd.Any())
        {
            context.Roles.AddRange(rolesToAdd);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAdmin(ApplicationDbContext context, IAuthService authService)
    {
        var admin = await context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Username == "admin");

        if (admin == null)
        {
            admin = new User
            {
                Username = "admin",
                Email = "admin@textile.local",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PasswordHash = authService.HashPassword("Admin")
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        var adminRole = await context.Roles
            .FirstAsync(r => r.Name == RolesConstants.Admin);

        var hasRole = await context.UserRoles
            .AnyAsync(ur => ur.UserId == admin.Id && ur.RoleId == adminRole.Id);

        if (!hasRole)
        {
            context.UserRoles.Add(new UserRole
            {
                UserId = admin.Id,
                RoleId = adminRole.Id,
                AssignedByUserId = admin.Id,
                AssignedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
}
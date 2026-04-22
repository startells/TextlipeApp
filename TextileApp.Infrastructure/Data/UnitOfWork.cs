using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TextileApp.Infrastructure.Data.Repositories;

namespace TextileApp.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<UnitOfWork> _logger;
    private IUserRepository? _userRepository;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<UnitOfWork>();
    }
    
    public IUserRepository Users => _userRepository ??= new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await (_transaction?.CommitAsync() ?? Task.CompletedTask);
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await (_transaction?.RollbackAsync() ?? Task.CompletedTask);
        }
        finally
        {
            _transaction?.Dispose();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
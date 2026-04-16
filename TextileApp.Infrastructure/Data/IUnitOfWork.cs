using TextileApp.Infrastructure.Data.Repositories;

namespace TextileApp.Infrastructure.Data;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
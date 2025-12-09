    namespace ErpManagement.Domain.Interfaces;

public interface IDatabaseTransaction : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}

namespace GrpcCommunication.API.Domain.DomainObjects;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T obj);
    Task AddAsync(T obj);
    Task SaveAsync();
}
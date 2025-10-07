using System.Linq.Expressions;

namespace MultiDatabase.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(object id);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null!);
}
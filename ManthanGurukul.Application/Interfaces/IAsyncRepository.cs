using System.Linq.Expressions;

namespace ManthanGurukul.Application.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        Task<T> GetLastOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}

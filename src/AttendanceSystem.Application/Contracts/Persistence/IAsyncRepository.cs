using AttendanceSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Contracts.Persistence
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IReadOnlyList<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions);

        Task<PagedResult<TEntity>> GetPagedFilteredAsync(Expression<Func<TEntity, bool>> filter, int page, int pageSize, string sortColumn, string sortOrder,
             bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);

        Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector, bool includeDeleted = false);

        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
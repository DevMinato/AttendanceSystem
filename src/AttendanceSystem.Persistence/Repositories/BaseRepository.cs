using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceSystem.Persistence.Repositories
{

    public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = _dbSet.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = _dbSet.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = _dbSet.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task<PagedResult<TEntity>> GetPagedFilteredAsync(Expression<Func<TEntity, bool>> filter, int page, int pageSize, string sortColumn, string sortOrder,
              bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = _dbSet.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = ApplySorting(query, sortColumn, sortOrder);

            int totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<TEntity> { Items = items, TotalCount = totalCount };
        }


        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            return await query.CountAsync(predicate);
        }

        public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            return await query.Where(predicate).SumAsync(selector);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortColumn, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, sortColumn);
            var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), parameter);

            if (sortOrder?.ToLower() == "desc")
            {
                query = query.OrderByDescending(lambda);
            }
            else
            {
                query = query.OrderBy(lambda);
            }

            return query;
        }
    }
}
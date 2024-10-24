using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjProcessOrders.Domain.Bases;
using ProjProcessOrders.Infrastructure.Context;
using ProjProcessOrders.UseCase.Interfaces;
using System.Linq.Expressions;

namespace ProjProcessOrders.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKeyType> : IGenericRepository<TEntity, TKeyType>
        where TEntity : BaseEntity<TKeyType>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GenericRepository<TEntity, TKeyType>> _logger;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext, ILogger<GenericRepository<TEntity, TKeyType>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> Update(TEntity obj, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return obj;
        }

        public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return QueryableModel<TEntity, TKeyType>(where, includes);
        }

        public IQueryable<TModel> QueryableModel<TModel, TKeyModel>(Expression<Func<TModel, bool>> where, Func<IQueryable<TModel>, IQueryable<TModel>> includes = null) where TModel : BaseEntity<TKeyModel>
        {
            var query = _dbContext.Set<TModel>().AsQueryable();
            if (where != null)
                query = query.Where(where);
            if (includes != null)
                query = includes(query);
            return query;
        }

        public async Task<TEntity> Insert(TEntity obj, CancellationToken cancellationToken = default)
        {
            _dbSet.Add(obj);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return obj;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

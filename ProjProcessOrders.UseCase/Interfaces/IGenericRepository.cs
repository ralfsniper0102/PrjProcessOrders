using ProjProcessOrders.Domain.Bases;
using System.Linq.Expressions;

namespace ProjProcessOrders.UseCase.Interfaces
{
    public interface IGenericRepository<TEntity, TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        Task<TEntity> Update(TEntity obj, CancellationToken cancellationToken = default);
        IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<TEntity> Insert(TEntity obj, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

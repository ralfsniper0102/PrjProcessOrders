using ProjProcessOrders.Domain.Bases;
using System.Linq.Expressions;

namespace ProjProcessOrders.UseCase.Interfaces
{
    public interface IGenericRepository<TEntity, TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        Task<TEntity> UpdateAsync(TEntity obj, CancellationToken cancellationToken = default);
        IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<TEntity> InsertAsync(TEntity obj, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity obj, CancellationToken cancellationToken = default);
    }
}

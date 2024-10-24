using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjProcessOrders.Domain.Bases;

namespace ProjProcessOrders.Persistence.Bases
{
    public static class BaseEntityMap
    {
        public static void Configure<TEntity, TKey>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity<TKey>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CreatedAt).HasDefaultValueSql("current_timestamp");
        }
    }
}

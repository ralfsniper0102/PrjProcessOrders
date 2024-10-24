using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Persistence.Bases;

namespace ProjProcessOrders.Persistence.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            BaseEntityMap.Configure<Order, int>(builder);
            builder.ToTable("order", "app_order");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.OrderProducts)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        }
    }
}

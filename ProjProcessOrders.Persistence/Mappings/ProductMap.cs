using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Persistence.Bases;

namespace ProjProcessOrders.Persistence.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            BaseEntityMap.Configure<Product, int>(builder);
            builder.ToTable("product", "app_product");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.OrderProducts)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        }
    }
}

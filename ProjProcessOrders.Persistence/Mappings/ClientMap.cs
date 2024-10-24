using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Persistence.Bases;

namespace ProjProcessOrders.Persistence.Mappings
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            BaseEntityMap.Configure<Client, int>(builder);
            builder.ToTable("client", "app_client");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Orders)
               .WithOne(x => x.Client)
               .HasForeignKey(x => x.ClientId);
        }
    }
}

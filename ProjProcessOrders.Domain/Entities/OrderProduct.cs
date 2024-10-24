using ProjProcessOrders.Domain.Bases;

namespace ProjProcessOrders.Domain.Entities
{
    public class OrderProduct : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}

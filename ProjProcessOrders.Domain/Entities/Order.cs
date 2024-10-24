using ProjProcessOrders.Domain.Bases;

namespace ProjProcessOrders.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }


        public List<OrderProduct> OrderProducts { get; set; }
    }
}

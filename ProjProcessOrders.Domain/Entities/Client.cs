using ProjProcessOrders.Domain.Bases;

namespace ProjProcessOrders.Domain.Entities
{
    public class Client : BaseEntity<int>
    {
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
    }
}

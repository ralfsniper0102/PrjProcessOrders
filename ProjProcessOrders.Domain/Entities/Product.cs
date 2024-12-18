using ProjProcessOrders.Domain.Bases;

namespace ProjProcessOrders.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        
        public List<OrderProduct> OrderProducts { get; set; }
    }
}

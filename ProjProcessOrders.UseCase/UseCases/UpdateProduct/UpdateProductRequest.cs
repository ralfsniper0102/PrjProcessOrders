using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.UpdateProduct
{
    public class UpdateProductRequest : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}

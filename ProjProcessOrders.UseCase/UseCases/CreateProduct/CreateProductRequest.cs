using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.CreateProduct
{
    public class CreateProductRequest : IRequest<CreateProductResponse>
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}

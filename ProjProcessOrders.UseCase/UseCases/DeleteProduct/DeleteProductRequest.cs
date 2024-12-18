using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.DeleteProduct
{
    public class DeleteProductRequest : IRequest<Unit>
    {
        public int ProductId { get; set; }
    }
}

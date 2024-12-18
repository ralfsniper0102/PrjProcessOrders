using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetProductById
{
    public class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
        public int ProductId { get; set; }
    }
}

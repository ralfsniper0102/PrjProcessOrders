using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderById
{
    public class GetOrderByIdRequest : IRequest<GetOrderByIdResponse>
    {
        public int OrderId { get; set; }
    }
}

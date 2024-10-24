using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderValue
{
    public class GetOrderValueRequest : IRequest<GetOrderValueResponse>
    {
        public int OrderId { get; set; }
    }
}

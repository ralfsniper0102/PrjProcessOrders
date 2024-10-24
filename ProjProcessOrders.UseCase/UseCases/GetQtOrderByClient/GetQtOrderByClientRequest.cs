using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient
{
    public class GetQtOrderByClientRequest : IRequest<GetQtOrderByClientResponse>
    {
        public int ClientId { get; set; }
    }
}

using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetListOrderByClient
{
    public class GetListOrderByClientRequest : IRequest<GetListOrderByClientResponse>
    {
        public int ClientId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetClients
{
    public class GetClientsRequest : IRequest<GetClientsResponse>
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

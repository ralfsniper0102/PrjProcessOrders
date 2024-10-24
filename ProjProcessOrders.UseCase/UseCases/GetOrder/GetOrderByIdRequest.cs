using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetOrder
{
    public class GetOrderByIdRequest : IRequest<GetOrderByIdResponse>
    {
        public int OrderId { get; set; }
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

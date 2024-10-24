using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetProducts
{
    public class GetProductsRequest : IRequest<GetProductsResponse>
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

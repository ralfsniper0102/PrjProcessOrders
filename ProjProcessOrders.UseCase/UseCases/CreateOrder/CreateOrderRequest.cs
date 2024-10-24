using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.CreateOrder
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public int ClientId { get; set; }
        public List<OrderProductViewModel> OrderProducts { get; set; }
    }

    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

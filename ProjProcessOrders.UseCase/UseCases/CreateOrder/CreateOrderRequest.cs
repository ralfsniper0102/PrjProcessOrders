using MediatR;
using ProjProcessOrders.UseCase.DTO;

namespace ProjProcessOrders.UseCase.UseCases.CreateOrder
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public int ClientId { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    } 
}

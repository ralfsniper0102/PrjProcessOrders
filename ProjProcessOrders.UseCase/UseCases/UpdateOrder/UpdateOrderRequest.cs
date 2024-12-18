using MediatR;
using ProjProcessOrders.UseCase.DTO;

namespace ProjProcessOrders.UseCase.UseCases.UpdateOrder
{
    public class UpdateOrderRequest : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    }
}

using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.DeleteOrder;

public class DeleteOrderRequest : IRequest<Unit>
{
    public int OrderId { get; set; }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.DeleteOrder;

public class DeleteOrderRequestHandler : IRequestHandler<DeleteOrderRequest, Unit>
{
    private readonly IGenericRepository<Order, int> _orderRepository;
    private readonly Resources _resources;
    
    public DeleteOrderRequestHandler(IGenericRepository<Order, int> orderRepository, Resources resources)
    {
        _orderRepository = orderRepository;
        _resources = resources;
    }
    
    public async Task<Unit> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.Queryable(x => x.Id == request.OrderId).FirstOrDefaultAsync();
        
        if (result == null)
            throw new ConflictException(_resources.OrderNotExists());
        
        await _orderRepository.DeleteAsync(result);

        return Unit.Value;
    }
}
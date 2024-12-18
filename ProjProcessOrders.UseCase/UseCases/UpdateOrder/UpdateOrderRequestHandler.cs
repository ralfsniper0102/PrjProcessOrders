using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.UpdateOrder
{
    public class UpdateOrderRequestHandler : IRequestHandler<UpdateOrderRequest, Unit>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly Resources _resources;

        public UpdateOrderRequestHandler(IGenericRepository<Order, int> orderRepository, IGenericRepository<Product, int> productRepository, Resources resources)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderRepository.Queryable(x => x.Id == request.OrderId).Include(x => x.OrderProducts).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.OrderNotExists());
            
            var products = await _productRepository.Queryable(x => x.Id > 0).ToListAsync();

            foreach (var orderProduct in request.OrderProducts)
            {
                var product = products.FirstOrDefault(x => x.Id == orderProduct.ProductId);
                if (product == null)
                    throw new ConflictException(_resources.ProductNotExists());
            }

            result.OrderProducts = request.OrderProducts.Select(x => new OrderProduct
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();

            var productIds = request.OrderProducts.GroupBy(x => x.ProductId)
                                     .Where(g => g.Count() > 1)
                                     .Select(g => g.Key)
                                     .ToList();
            
            if(productIds.Count > 0)
                throw new ConflictException(_resources.DuplicateProduct());
            
            await _orderRepository.UpdateAsync(result, cancellationToken);
            
            return Unit.Value;
        }
    }
}

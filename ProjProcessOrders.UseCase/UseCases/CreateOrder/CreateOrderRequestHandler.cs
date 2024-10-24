using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.CreateOrder
{
    public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public CreateOrderRequestHandler(
            IGenericRepository<Order, int> orderRepository,
            IGenericRepository<Client, int> clientRepository,
            IGenericRepository<Product, int> productRepository,
            IMapper mapper,
            Resources resources
            )
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var query = await _clientRepository
                                                .Queryable(x => x.Id == request.ClientId)
                                                .AnyAsync(cancellationToken);

            if (!query)
                throw new ConflictException(_resources.ClientNotExists());

            var productIds = request.OrderProducts.Select(x => x.ProductId).Distinct().ToList();
            var productsExist = await _productRepository
                                                    .Queryable(p => productIds.Contains(p.Id))
                                                    .ToListAsync(cancellationToken);

            if (productsExist.Count != productIds.Count)
                throw new ConflictException(_resources.ProductNotRegistered());

            var newOrder = new Order
            {
                ClientId = request.ClientId,
                OrderProducts = request.OrderProducts.Select(x => new OrderProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var createdOrder = await _orderRepository.Insert(newOrder, cancellationToken);

            return new CreateOrderResponse { Id = createdOrder.Id };
        }
    }
}

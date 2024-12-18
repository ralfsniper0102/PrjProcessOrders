using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderById
{
    public class GetOrderByIdRequestHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public GetOrderByIdRequestHandler(IGenericRepository<Order, int> orderRepository, IMapper mapper, Resources resources)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var query = _orderRepository.Queryable(x => x.Id == request.OrderId)
                                        .Include(x => x.Client)
                                        .Include(x => x.OrderProducts)
                                        .ThenInclude(x => x.Product)
                                        .AsNoTracking()
                                        .FirstOrDefault();

            if (query == null)
                throw new ConflictException(_resources.OrdersNotExists());

            List<OrderItemViewModel> listOrderProducts = new List<OrderItemViewModel>();

            foreach (var item in query.OrderProducts)
            {
                listOrderProducts.Add(new OrderItemViewModel
                {
                    Produto = item.Product.ProductName,
                    Quantidade = item.Product.ProductQuantity,
                    Preco = item.Product.ProductPrice
                });
            }

            return new GetOrderByIdResponse
            {
                CodigoPedido = query.Id,
                CodigoCliente = query.ClientId,
                Items = listOrderProducts
            };
        }
    }
}

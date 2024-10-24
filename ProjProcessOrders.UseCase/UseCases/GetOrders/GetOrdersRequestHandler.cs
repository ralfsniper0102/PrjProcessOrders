using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetOrders
{
    public class GetOrdersRequestHandler : IRequestHandler<GetOrdersRequest, GetOrdersResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersRequestHandler(IGenericRepository<Order, int> orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            int skip = Math.Max(page - 1, 0) * pageSize;

            var query = await _orderRepository.Queryable(x => x.Id > 0)
                                               .OrderByDescending(x => x.Id)
                                               .Include(x => x.Client)
                                               .Include(x => x.OrderProducts)
                                                    .ThenInclude(x => x.Product)
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken: cancellationToken);

            switch (request.Search)
            {
                case "":
                    break;
                default:
                    query = query.Where(order =>
                    order.Id.ToString().Contains(request.Search.ToLower()) ||
                    order.Client.Name.ToLower().Contains(request.Search.ToLower())
                    )
                        .Distinct()
                        .ToList();
                    break;
            }

            var qt = query.Count();
            var qtPages = (int)Math.Ceiling((double)qt / pageSize);

            query = query
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            List<OrdersViewModel> listOrderViewModels = new List<OrdersViewModel>();

            foreach (var order in query)
            {
                List<OrdersItemViewModel> listOrderProducts = new List<OrdersItemViewModel>();

                foreach (var item in order.OrderProducts)
                {
                    listOrderProducts.Add(new OrdersItemViewModel
                    {
                        Produto = item.Product.ProductName,
                        Quantidade = item.Product.ProductQuantity,
                        Preco = item.Product.ProductPrice
                    });
                }

                listOrderViewModels.Add(new OrdersViewModel
                {
                    CodigoPedido = order.Id,
                    CodigoCliente = order.ClientId,
                    Items = listOrderProducts 
                });
            }

            return new GetOrdersResponse
            {
                Pedidos = listOrderViewModels,
                Page = page,
                PageSize = pageSize,
                QtTotal = qt,
                QtPages = Math.Max(qtPages, 1),
            };
        }
    }
}

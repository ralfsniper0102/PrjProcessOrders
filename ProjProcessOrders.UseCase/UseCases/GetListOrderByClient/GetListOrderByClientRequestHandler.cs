using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetListOrderByClient
{
    public class GetListOrderByClientRequestHandler : IRequestHandler<GetListOrderByClientRequest, GetListOrderByClientResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public GetListOrderByClientRequestHandler(IGenericRepository<Order, int> orderRepository, IMapper mapper, Resources resources)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetListOrderByClientResponse> Handle(GetListOrderByClientRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            int skip = Math.Max(page - 1, 0) * pageSize;

            var query = await _orderRepository.Queryable(x => x.ClientId == request.ClientId)
                                               .Include(x => x.Client)
                                               .Include(x => x.OrderProducts)
                                                    .ThenInclude(x => x.Product)
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken: cancellationToken);

            if (query == null)
                throw new ConflictException(_resources.ClientNotExists());

            var qt = query.Count();
            var qtPages = (int)Math.Ceiling((double)qt / pageSize);

            query = query
                .Skip(skip)
                .Take(pageSize)
                .ToList();

             
            List<ListOrderViewModel> listOrderViewModels = new List<ListOrderViewModel>();

            foreach (var order in query)
            {
                List<ListOrderItemViewModel> listOrderProducts = new List<ListOrderItemViewModel>();

                foreach (var item in order.OrderProducts)
                {
                    listOrderProducts.Add(new ListOrderItemViewModel
                    {
                        Produto = item.Product.ProductName,
                        Quantidade = item.Product.ProductQuantity,
                        Preco = item.Product.ProductPrice
                    });
                }

                listOrderViewModels.Add(new ListOrderViewModel
                {
                    CodigoPedido = order.Id,
                    CodigoCliente = order.ClientId,
                    Items = listOrderProducts
                });
            }

            return new GetListOrderByClientResponse
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

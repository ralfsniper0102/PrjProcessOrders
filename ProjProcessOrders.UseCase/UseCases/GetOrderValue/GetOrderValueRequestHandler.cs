using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;


namespace ProjProcessOrders.UseCase.UseCases.GetOrderValue
{
    public class GetOrderValueRequestHandler : IRequestHandler<GetOrderValueRequest, GetOrderValueResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public GetOrderValueRequestHandler(IGenericRepository<Order, int> orderRepository, IMapper mapper, Resources resources)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetOrderValueResponse> Handle(GetOrderValueRequest request, CancellationToken cancellationToken)
        {
            var query = _orderRepository.Queryable(x => x.Id == request.OrderId)
                                        .Include(x => x.Client)
                                        .Include(x => x.OrderProducts)
                                        .ThenInclude(x => x.Product)
                                        .AsNoTracking()
                                        .FirstOrDefault();

            if (query == null)
                throw new ConflictException(_resources.OrdersNotExists());

            decimal totalValue = 0;

            foreach (var item in query.OrderProducts)
                totalValue = item.Product.ProductPrice * item.Product.ProductQuantity;
            
            return new GetOrderValueResponse
            {
                TotalValueOrder = totalValue
            };
        }
    }
}

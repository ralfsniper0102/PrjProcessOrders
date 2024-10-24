using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient
{
    public class GetQtOrderByClientRequestHandler : IRequestHandler<GetQtOrderByClientRequest, GetQtOrderByClientResponse>
    {
        private readonly IGenericRepository<Order, int> _orderRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public GetQtOrderByClientRequestHandler(IGenericRepository<Order, int> orderRepository, IMapper mapper, Resources resources)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetQtOrderByClientResponse> Handle(GetQtOrderByClientRequest request, CancellationToken cancellationToken)
        {
            var query = _orderRepository.Queryable(x => x.ClientId == request.ClientId)
                                        .Include(x => x.Client)
                                        .Include(x => x.OrderProducts)
                                        .ThenInclude(x => x.Product)
                                        .AsNoTracking()
                                        .FirstOrDefault();

            if (query == null)
                throw new ConflictException(_resources.ClientNotExists());

            return new GetQtOrderByClientResponse
            {
                QtOrder = query.OrderProducts.Count
            };
        }
    }
}

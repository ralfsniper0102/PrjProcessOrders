using AutoMapper;
using MediatR;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetProductById
{
    public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IGenericRepository<Product, int> _producttRepository;
        private readonly Resources _resources;
        private readonly IMapper _mapper;

        public GetProductByIdRequestHandler(IGenericRepository<Product, int> producttRepository, IMapper mapper, Resources resources)
        {
            _producttRepository = producttRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = _producttRepository.Queryable(x => x.Id == request.ProductId).FirstOrDefault();

            if (product == null)
                throw new ConflictException(_resources.ProductNotExists());

            return _mapper.Map<GetProductByIdResponse>(product);
        }
    }
}

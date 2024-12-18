using AutoMapper;
using MediatR;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;

namespace ProjProcessOrders.UseCase.UseCases.CreateProduct
{
    public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public CreateProductRequestHandler(IGenericRepository<Product, int> productRepository, IMapper mapper, Resources resources)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.Queryable(x => x.ProductName == request.ProductName)
                                    .ToListAsync(cancellationToken: cancellationToken);

            if (result.Count > 0)
                throw new ConflictException(_resources.NameAlreadyExists());

            var newProduct = _mapper.Map<Product>(request);
            var res = await _productRepository.InsertAsync(newProduct, cancellationToken);

            var createProductResponse = new CreateProductResponse();
            createProductResponse.Id = res.Id;

            return createProductResponse;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.UpdateProduct
{
    public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, Unit>
    {
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly Resources _resources;

        public UpdateProductRequestHandler(IGenericRepository<Product, int> productRepository, Resources resources)
        {
            _productRepository = productRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.Queryable(x => x.Id == request.ProductId).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ProductNotExists());

            result.ProductName = request.ProductName;
            result.ProductPrice = request.ProductPrice;
            result.ProductQuantity = request.ProductQuantity;

            await _productRepository.UpdateAsync(result, cancellationToken);

            return Unit.Value;
        }
    }
}

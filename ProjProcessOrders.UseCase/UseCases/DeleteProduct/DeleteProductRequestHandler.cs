using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.DeleteProduct
{
    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, Unit>
    {
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly Resources _resources;

        public DeleteProductRequestHandler(IGenericRepository<Product, int> productRepository, Resources resources)
        {
            _productRepository = productRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.Queryable(x => x.Id == request.ProductId).Include(x => x.OrderProducts).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ProductNotExists());

            if (result.OrderProducts.Count > 0)
                throw new ConflictException(_resources.ProductHasOrders());

            await _productRepository.DeleteAsync(result);

            return Unit.Value;
        }
    }
}

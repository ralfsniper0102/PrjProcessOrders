using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetProducts
{
    public class GetProductsRequestHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
    {
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly IMapper _mapper;

        public GetProductsRequestHandler(IGenericRepository<Product, int> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            int skip = Math.Max(page - 1, 0) * pageSize;

            var query = await _productRepository.Queryable(x => x.Id > 0)
                                               .OrderByDescending(x => x.Id)
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken: cancellationToken);

            switch (request.Search)
            {
                case "":
                    break;
                default:
                    query = query.Where(product =>
                    product.Id.ToString().Contains(request.Search.ToLower()) ||
                    product.ProductName.ToLower().Contains(request.Search.ToLower()) ||
                    product.ProductQuantity.ToString().Contains(request.Search.ToLower()) ||
                    product.ProductPrice.ToString().Contains(request.Search.ToLower())
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

            List<ProductViewModel> products = new List<ProductViewModel>();

            foreach (var product in query)
            {
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductQuantity = product.ProductQuantity,
                    ProductPrice = product.ProductPrice
                };

                products.Add(productViewModel);
            }

            GetProductsResponse getProductsResponse = new GetProductsResponse
            {
                Page = page,
                PageSize = pageSize,
                QtTotal = qt,
                QtPages = Math.Max(qtPages, 1),
                Products = products
            };

            return getProductsResponse;
        }
    }
}

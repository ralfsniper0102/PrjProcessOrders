using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.UseCase.UseCases.DeleteProduct;
using ProjProcessOrders.UseCase.UseCases.GetProductById;
using ProjProcessOrders.UseCase.UseCases.GetProducts;
using ProjProcessOrders.UseCase.UseCases.UpdateProduct;
using System.Net;

namespace ProjProcessOrders.ProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController<ProductController>
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator, Serilog.ILogger logger) : base(logger, mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateProduct")]
        [ProducesResponseType(typeof(CreateProductResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpGet("GetProductById/{id}")]
        [ProducesResponseType(typeof(GetProductByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int id)
        {
            return await CreateActionResult(new GetProductByIdRequest { ProductId = id });
        }

        [HttpGet("GetProducts")]
        [ProducesResponseType(typeof(GetProductsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts(string search = "", int page = 1, int pageSize = 10)
        {
            return await CreateActionResult(new GetProductsRequest { Search = search, Page = page, PageSize = pageSize });
        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(Unit),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(int id)
        { 
            return await CreateActionResult(new DeleteProductRequest { ProductId = id });
        }
    }
}

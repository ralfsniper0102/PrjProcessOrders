using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.Messaging;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.UseCase.UseCases.DeleteProduct;
using ProjProcessOrders.UseCase.UseCases.GetProductById;
using ProjProcessOrders.UseCase.UseCases.GetProducts;
using ProjProcessOrders.UseCase.UseCases.UpdateProduct;
using System.Net;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        public ProductController(RabbitMqClientService rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpPost("CreateProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            return await HandleRequestAsync<CreateProductRequest, CreateProductResponse>(request);
        }

        [HttpGet("GetProductById/{id}")]
        [ProducesResponseType(typeof(GetProductByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int id)
        {
            return await HandleRequestAsync<GetProductByIdRequest, GetProductByIdResponse>(new GetProductByIdRequest { ProductId = id });
        }

        [HttpGet("GetProducts")]
        [ProducesResponseType(typeof(GetProductsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts(string search = "", int page = 1, int pageSize = 10)
        {
            return await HandleRequestAsync<GetProductsRequest, GetProductsResponse>(new GetProductsRequest { Search = search, Page = page, PageSize = pageSize });
        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(Unit),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            return await HandleRequestAsync<UpdateProductRequest, Unit>(request);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return await HandleRequestAsync<DeleteProductRequest, Unit>(new DeleteProductRequest { ProductId = id });
        }
    }
}

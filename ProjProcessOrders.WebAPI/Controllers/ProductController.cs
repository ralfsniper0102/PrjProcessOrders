using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.WebAPI.Infrastructure.Messaging;
using System.Net;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        public ProductController(RabbitMqServiceWebAPI rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpPost("CreateProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            return await HandleRequestAsync<CreateProductRequest, CreateProductResponse>(request);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
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
    }
}

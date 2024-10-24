using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.GetOrderValue;
using ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient;
using System.Net;

namespace ProjProcessOrders.ProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController<OrderController>
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator, Serilog.ILogger logger) : base(logger, mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetOrderValue")]
        [ProducesResponseType(typeof(GetOrderValueResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderValue([FromQuery] GetOrderValueRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpGet("GetQtOrderByClient")]
        [ProducesResponseType(typeof(GetQtOrderByClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQtOrderByClient([FromQuery] GetQtOrderByClientRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(CreateOrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var response = await CreateActionResult(request);

            return response;
        }
    }
}

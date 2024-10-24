using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.WebAPI.Infrastructure.Messaging;
using System.Net;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : BaseController<OrdersController>
    {
        public OrdersController(RabbitMqServiceWebAPI rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpPost("CreateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            return await HandleRequestAsync<CreateOrderRequest, CreateOrderResponse>(request);
        }
    }
}

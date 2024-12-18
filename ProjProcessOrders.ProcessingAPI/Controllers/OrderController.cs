using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.GetOrders;
using ProjProcessOrders.UseCase.UseCases.GetOrderValue;
using ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient;
using ProjProcessOrders.UseCase.UseCases.UpdateOrder;
using System.Net;
using ProjProcessOrders.UseCase.UseCases.DeleteOrder;
using ProjProcessOrders.UseCase.UseCases.GetListOrderByClient;
using ProjProcessOrders.UseCase.UseCases.GetOrderById;

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

        [HttpGet("GetOrderValue/{id}")]
        [ProducesResponseType(typeof(GetOrderValueResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderValue(int id)
        {
            return await CreateActionResult(new GetOrderValueRequest { OrderId = id });
        }

        [HttpGet("GetQtOrderByClient/{id}")]
        [ProducesResponseType(typeof(GetQtOrderByClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQtOrderByClient(int id)
        {
            return await CreateActionResult(new GetQtOrderByClientRequest { ClientId = id });
        }

        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(CreateOrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var response = await CreateActionResult(request);

            return response;
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(GetOrdersResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders(string search = "", int page = 1, int pageSize = 10)
        {
            return await CreateActionResult(new GetOrdersRequest { Search = search, Page = page, PageSize = pageSize });
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpGet("GetOrderById/{id}")]
        [ProducesResponseType(typeof(GetOrderByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return await CreateActionResult(new GetOrderByIdRequest { OrderId = id });
        }

        [HttpGet("GetListOrderByClient")]
        [ProducesResponseType(typeof(GetListOrderByClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListOrderByClient(int clientId, int page = 1, int pageSize = 10)
        {
            return await CreateActionResult(new GetListOrderByClientRequest
                { ClientId = clientId, Page = page, PageSize = pageSize });
        }

        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await CreateActionResult(new DeleteOrderRequest { OrderId = id });
        }
    }
}
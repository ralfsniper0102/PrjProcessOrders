using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.Messaging;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.GetOrders;
using ProjProcessOrders.UseCase.UseCases.GetOrderValue;
using ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient;
using System.Net;
using MediatR;
using ProjProcessOrders.UseCase.UseCases.DeleteOrder;
using ProjProcessOrders.UseCase.UseCases.GetListOrderByClient;
using ProjProcessOrders.UseCase.UseCases.GetOrderById;
using ProjProcessOrders.UseCase.UseCases.UpdateOrder;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : BaseController<OrdersController>
    {
        public OrdersController(RabbitMqClientService rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpGet("GetOrderValue/{id}")]
        [ProducesResponseType(typeof(GetOrderValueResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderValue(int id)
        {
            return await HandleRequestAsync<GetOrderValueRequest, GetOrderValueResponse>(new GetOrderValueRequest { OrderId = id});
        }

        [HttpGet("GetQtOrderByClient/{id}")]
        [ProducesResponseType(typeof(GetQtOrderByClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQtOrderByClient(int id)
        {
            return await HandleRequestAsync<GetQtOrderByClientRequest, GetQtOrderByClientResponse>(new GetQtOrderByClientRequest { ClientId = id });
        }

        [HttpPost("CreateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            return await HandleRequestAsync<CreateOrderRequest, CreateOrderResponse>(request);
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(GetOrdersResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders(string search = "", int page = 1, int pageSize = 10)
        {
            return await HandleRequestAsync<GetOrdersRequest, GetOrdersResponse>(new GetOrdersRequest { Search = search, Page = page, PageSize = pageSize });
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            return await HandleRequestAsync<UpdateOrderRequest, Unit>(request);
        }

        [HttpGet("GetOrderById/{id}")]
        [ProducesResponseType(typeof(GetOrderByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return await HandleRequestAsync<GetOrderByIdRequest, GetOrderByIdResponse>(new GetOrderByIdRequest
                { OrderId = id });
        }
        
        [HttpGet("GetListOrderByClient")]
        [ProducesResponseType(typeof(GetListOrderByClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListOrderByClient(int clientId, int page = 1, int pageSize = 10)
        {
            return await HandleRequestAsync<GetListOrderByClientRequest, GetListOrderByClientResponse>(new GetListOrderByClientRequest
                { ClientId = clientId, Page = page, PageSize = pageSize });
        }

        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await HandleRequestAsync<DeleteOrderRequest, Unit>(new DeleteOrderRequest { OrderId = id });
        }
    }
}

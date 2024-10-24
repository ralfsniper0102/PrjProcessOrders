using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.WebAPI.Infrastructure.Messaging;
using System.Net;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController<ClientController>
    {
        public ClientController(RabbitMqServiceWebAPI rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpPost("CreateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            return await HandleRequestAsync<CreateClientRequest, CreateClientResponse>(request);
        }
    }
}

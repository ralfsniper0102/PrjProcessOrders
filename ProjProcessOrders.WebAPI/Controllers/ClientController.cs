using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.Messaging;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.DeleteClient;
using ProjProcessOrders.UseCase.UseCases.GetClientById;
using ProjProcessOrders.UseCase.UseCases.GetClients;
using ProjProcessOrders.UseCase.UseCases.UpdateClient;
using System.Net;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController<ClientController>
    {
        public ClientController(RabbitMqClientService rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpGet("GetClients")]
        [ProducesResponseType(typeof(GetClientsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClients(string search = "", int page = 1, int pageSize = 10)
        {
            var request = new GetClientsRequest { Search = search, Page = page, PageSize = pageSize };

            return await HandleRequestAsync<GetClientsRequest, GetClientsResponse>(request);
        }
        
        [HttpPost("CreateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            return await HandleRequestAsync<CreateClientRequest, CreateClientResponse>(request);
        }
        
        [HttpGet("GetClientById/{id}")]
        [ProducesResponseType(typeof(GetClientByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClientById(int id)
        {
            var request = new GetClientByIdRequest { Id = id };

            return await HandleRequestAsync<GetClientByIdRequest, GetClientByIdResponse>(request);
        }

        [HttpPut("UpdateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            return await HandleRequestAsync<UpdateClientRequest, Unit>(request);
        }

        [HttpDelete("DeleteClient/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return await HandleRequestAsync<DeleteClientRequest, Unit>(new DeleteClientRequest { Id = id });
        }
    }
}

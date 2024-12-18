using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.DeleteClient;
using ProjProcessOrders.UseCase.UseCases.GetClientById;
using ProjProcessOrders.UseCase.UseCases.GetClients;
using ProjProcessOrders.UseCase.UseCases.UpdateClient;
using System.Net;

namespace ProjProcessOrders.ProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseApiController<ClientController>
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator, Serilog.ILogger logger) : base(logger, mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetClients")]
        [ProducesResponseType(typeof(GetClientsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClients(string search = "", int page = 1, int pageSize = 10)
        {
            return await CreateActionResult(new GetClientsRequest { Search = search, Page = page, PageSize = pageSize });
        }

        [HttpPost("CreateClient")]
        [ProducesResponseType(typeof(CreateClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpGet("GetClientById/{id}")]
        [ProducesResponseType(typeof(GetClientByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClientById(int id)
        {
            return await CreateActionResult(new GetClientByIdRequest { Id = id });
        }

        [HttpPut("UpdateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpDelete("DeleteClient/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return await CreateActionResult(new DeleteClientRequest { Id = id });
        }
    }
}

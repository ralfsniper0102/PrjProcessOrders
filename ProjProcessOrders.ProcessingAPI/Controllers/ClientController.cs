using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.GetClients;
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
            var response = await CreateActionResult(request);

            return response;
        }

        
    }
}

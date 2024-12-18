using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjProcessOrders.Messaging;
using ProjProcessOrders.Messaging.DTOs;
using ProjProcessOrders.UseCase.DTO;
using Serilog;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [ApiController]
    public abstract class BaseController<TControler> : ControllerBase
    {
        protected readonly RabbitMqClientService _rabbitMqService;
        protected readonly Serilog.ILogger _logger;

        protected BaseController(RabbitMqClientService rabbitMqService, Serilog.ILogger logger)
        {
            _logger = Log.ForContext<TControler>();
            _rabbitMqService = rabbitMqService;
        }

        protected async Task<IActionResult> HandleRequestAsync<TRequest, TResponse>(TRequest request)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();

            try
            {
                _logger.Information($"Enviando mensagem. CorrelationId: {correlationId} - {JsonConvert.SerializeObject(request)}");

                var result = await _rabbitMqService.SendMessageInChunksAsync(request, correlationId);

                var resultDeserialize = JsonConvert.DeserializeObject<ChunkMessageReturn>(result);

                switch (resultDeserialize.StatusCode)
                {
                    case 200:
                        var resultDeserializeResponse = JsonConvert.DeserializeObject<TResponse>(resultDeserialize.Body);
                        return Ok(resultDeserializeResponse);

                    case 409:
                        return StatusCode(409, resultDeserialize.Body);

                    case 412:
                        var resultDeserializeError = JsonConvert.DeserializeObject<ErrorDTO>(resultDeserialize.Body);
                        return StatusCode(412, resultDeserializeError);

                    default:
                        return StatusCode(500, resultDeserialize.Body);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Erro ao enviar mensagem. CorrelationId: {correlationId} - {JsonConvert.SerializeObject(request)}");
                return StatusCode(500, $"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}

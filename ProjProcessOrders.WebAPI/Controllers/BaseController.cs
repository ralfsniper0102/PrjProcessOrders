using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjProcessOrders.WebAPI.Infrastructure.Messaging;
using Serilog;

namespace ProjProcessOrders.WebAPI.Controllers
{
    [ApiController]
    public abstract class BaseController<TControler> : ControllerBase
    {
        protected readonly RabbitMqServiceWebAPI _rabbitMqService;
        protected readonly Serilog.ILogger _logger;

        protected BaseController(RabbitMqServiceWebAPI rabbitMqService, Serilog.ILogger logger)
        {
            _logger = Log.ForContext<TControler>();
            _rabbitMqService = rabbitMqService;
        }

        protected async Task<IActionResult> HandleRequestAsync<TRequest, TResponse>(TRequest request)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();

            try
            {
                _rabbitMqService.SendMessageInChunks(request, correlationId);

                _logger.Information($"Mensagem enviada com sucesso. CorrelationId: {correlationId} - {JsonConvert.SerializeObject(request)}");
                return Ok("Mensagem enviada com sucesso");
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Erro ao enviar mensagem. CorrelationId: {correlationId} - {JsonConvert.SerializeObject(request)}");
                return StatusCode(500, $"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}

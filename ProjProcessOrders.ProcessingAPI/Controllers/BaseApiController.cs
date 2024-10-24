using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjProcessOrders.Exception.Exceptions;
using Serilog;
using System.Net;
using System.Text.Json;

namespace ProjProcessOrders.ProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController<TControler> : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly Serilog.ILogger _logger;

        protected BaseApiController(Serilog.ILogger logger, IMediator mediator)
        {
            _logger = Log.ForContext<TControler>();
            _mediator = mediator;
        }

        protected async Task<IActionResult> CreateActionResult<T>(T model)
        {
            try
            {
                var result = await _mediator.Send(model);

                return Ok(result);
            }
            catch (PreconditionFailedException ex)
            {
                _logger.Information(ex, $"PreconditionFailedException: {JsonSerializer.Serialize(ex.BadRequestObjectResult).FormatLogSize()} on CreateActionResult model: {JsonSerializer.Serialize(model).FormatLogSize()}");
                return ex.BadRequestObjectResult;
            }
            catch (ConflictException ex)
            {
                _logger.Information(ex, $"ConflictException: {ex.Message} on CreateActionResult model: {JsonSerializer.Serialize(model).FormatLogSize()}");
                return new ConflictObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                };
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Exception: {ex.Message} on CreateActionResult model: {JsonSerializer.Serialize(model).FormatLogSize()}");
                return new ObjectResult(GetErrorResult(ex))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        private string GetErrorResult(System.Exception Exception)
        {
            var result = $"{{\"requestId\":\"{HttpContext.TraceIdentifier}\"";
            if (HttpContext.Request.Host.Host.Contains("localhost") || HttpContext.Request.Host.Host.Contains("service-hml"))
                result += $",\"Exception\":{GetDataFromException(Exception)}";
            result += "}";
            return result;
        }

        private string GetDataFromException(System.Exception Exception)
        {
            if (Exception == null)
                return "null";
            return $"{{\"message\":\"{Exception.Message.FormatLogSize()}\",\"stackTrace\":\"{Exception.StackTrace.FormatLogSize()}\",\"innerException\":{GetDataFromException(Exception.InnerException)}}}";
        }
    }

    public static class StringExtension
    {
        public static string FormatLogSize(this string model)
        {
            return model;
        }
    }
}

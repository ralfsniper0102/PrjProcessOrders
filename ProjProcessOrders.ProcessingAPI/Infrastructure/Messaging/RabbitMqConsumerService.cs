using MediatR;
using Newtonsoft.Json;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.ProcessingAPI.Infrastructure.Messaging;
using ProjProcessOrders.UseCase.Enums;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.UseCase.UseCases.GetOrders;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IModel _channel;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<string, TaskCompletionSource<GetOrdersResponse>> _responseAwaiters = new();
    private static Dictionary<string, List<ChunkMessage>> _chunksBuffer = new();
    private readonly string _queueName;
    private readonly Serilog.ILogger _logger;

    public RabbitMqConsumerService(IModel channel, IMediator mediator, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _channel = channel;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
        _queueName = configuration["RabbitMqSettings:QueueName"];
        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) => await HandleMessageAsync(ea, stoppingToken);
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        ChunkMessage? chunkMessage = DeserializeMessage(message);
        if (chunkMessage == null) return;

        if (!_chunksBuffer.ContainsKey(ea.BasicProperties.CorrelationId))
            _chunksBuffer[ea.BasicProperties.CorrelationId] = new List<ChunkMessage>();

        _chunksBuffer[ea.BasicProperties.CorrelationId].Add(chunkMessage);
        await ProcessChunks(ea, chunkMessage);
    }

    private ChunkMessage? DeserializeMessage(string message)
    {
        try
        {
            return JsonConvert.DeserializeObject<ChunkMessage>(message);
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, $"Erro ao desserializar a mensagem: {ex.Message}");
            return null;
        }
    }

    private async Task ProcessChunks(BasicDeliverEventArgs ea, ChunkMessage chunkMessage)
    {
        var correlationId = ea.BasicProperties.CorrelationId;

        if (_chunksBuffer[correlationId].Count == chunkMessage.TotalChunks)
        {
            var fullMessage = string.Concat(_chunksBuffer[correlationId]
                .OrderBy(chunk => chunk.CurrentChunk)
                .Select(chunk => Encoding.UTF8.GetString(chunk.Payload)));

            try
            {
                await ProcessFullMessage(fullMessage, ea, chunkMessage.RequestType);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Erro ao processar mensagem: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, false); 
                return;
            }

            _chunksBuffer.Remove(correlationId);
        }

        _channel.BasicAck(ea.DeliveryTag, false);
    }

    private async Task ProcessFullMessage(string fullMessage, BasicDeliverEventArgs ea, RequestTypeEnum requestType)
    {
        if (fullMessage == null) return;

        object? request = null;

        switch (requestType)
        {
            case RequestTypeEnum.CreateClientRequest:
                request = JsonConvert.DeserializeObject<CreateClientRequest>(fullMessage);
                break;

            case RequestTypeEnum.CreateProductRequest:
                request = JsonConvert.DeserializeObject<CreateProductRequest>(fullMessage);
                break;

            case RequestTypeEnum.CreateOrderRequest:
                request = JsonConvert.DeserializeObject<CreateOrderRequest>(fullMessage);
                break;

            default:
                Console.WriteLine($"Tipo de requisição desconhecida: {fullMessage}");
                _channel.BasicAck(ea.DeliveryTag, false);
                return;
        }

        if (request != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var response = default(object);
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                try
                {
                    response = await mediator.Send(request);
                }
                catch (PreconditionFailedException ex)
                {
                    _logger.Error(ex, $"PreconditionFailedException: {ex.Message})");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw;
                }
                catch (ConflictException ex)
                {
                    _logger.Error(ex, $"ConflictException: {ex.Message})");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Erro ao processar requisição: {ex.Message})");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw;
                }
            }
        }
    }
}
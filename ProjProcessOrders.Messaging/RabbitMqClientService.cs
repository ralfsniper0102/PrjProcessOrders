using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjProcessOrders.Messaging.DTOs;
using ProjProcessOrders.UseCase.Enums;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.UseCase.UseCases.DeleteClient;
using ProjProcessOrders.UseCase.UseCases.DeleteProduct;
using ProjProcessOrders.UseCase.UseCases.GetClientById;
using ProjProcessOrders.UseCase.UseCases.GetClients;
using ProjProcessOrders.UseCase.UseCases.GetProductById;
using ProjProcessOrders.UseCase.UseCases.GetProducts;
using ProjProcessOrders.UseCase.UseCases.UpdateClient;
using ProjProcessOrders.UseCase.UseCases.UpdateProduct;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ProjProcessOrders.UseCase.UseCases.DeleteOrder;
using ProjProcessOrders.UseCase.UseCases.GetListOrderByClient;
using ProjProcessOrders.UseCase.UseCases.GetOrderById;
using ProjProcessOrders.UseCase.UseCases.GetOrders;
using ProjProcessOrders.UseCase.UseCases.GetOrderValue;
using ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient;
using ProjProcessOrders.UseCase.UseCases.UpdateOrder;

namespace ProjProcessOrders.Messaging
{
    public class RabbitMqClientService
    {
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;
        private static Dictionary<string, List<ChunkMessageResponse>> _chunksBuffer = new();
        private readonly Serilog.ILogger _logger;
        private readonly string _responseQueue;


        public RabbitMqClientService(IModel channel, IConfiguration configuration, Serilog.ILogger logger)
        {
            _channel = channel;
            _configuration = configuration;
            _logger = logger;
            _channel.QueueDeclare(queue: _configuration["RabbitMqSettings:RequestQueueName"],
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _responseQueue = configuration["RabbitMqSettings:ResponseQueueName"];
        }

        public async Task<string> SendMessageInChunksAsync<T>(T request, string correlationId)
        {
            byte[] messageBytes = SerializeMessage(request);

            string responseQueueName = DeclareResponseQueue(correlationId, _responseQueue);

            TaskCompletionSource<string> tcs = InicialConsumer(responseQueueName, _channel);

            return await SendMessageInChunk(request, correlationId, messageBytes, responseQueueName, tcs);
        }

        private byte[] SerializeMessage<T>(T request)
        {
            _logger.Information("Serialização da mensagem...");

            var serializedRequest = JsonConvert.SerializeObject(request);
            return Encoding.UTF8.GetBytes(serializedRequest);
        }

        private string DeclareResponseQueue(string correlationId, string responseQueue)
        {
            _logger.Information("Definição da fila de resposta...");
            var responseQueueName = $"{responseQueue}_{correlationId}";

            _logger.Information("Declaração da fila de resposta...");
            var args = new Dictionary<string, object>
            {
                { "x-message-ttl", 15000 } 
            };

            _channel.QueueDeclare(queue: responseQueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: true,
                                 arguments: null);
            return responseQueueName;
        }

        private TaskCompletionSource<string> InicialConsumer(string responseQueueName, IModel channel)
        {
            _logger.Information("Declaração do consumidor...");
            var consumer = new EventingBasicConsumer(channel);
            var tcs = new TaskCompletionSource<string>();

            _logger.Information("Inicia o consumo das mensagens...");
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                _logger.Information("Deserialização da mensagem...");
                ChunkMessageResponse? chunkMessageResponse = DeserializeResponseMessage(message);

                if (chunkMessageResponse == null) //TODO: Testar essa condicao
                    throw new System.Exception("Mensagem nao pode ser desserializada");

                var correlationId = ea.BasicProperties.CorrelationId;
                if (!_chunksBuffer.ContainsKey(correlationId))
                    _chunksBuffer[correlationId] = new List<ChunkMessageResponse>();

                _logger.Information("Adicionando chunk ao buffer...");
                _chunksBuffer[correlationId].Add(chunkMessageResponse);

                _logger.Information("Verifica se todos os chunks foram recebidos...");
                if (_chunksBuffer[correlationId].Count == chunkMessageResponse.TotalChunks)
                {
                    _logger.Information("Todos os chunks foram recebidos...");

                    var fullMessage = string.Concat(_chunksBuffer[correlationId]
                        .OrderBy(chunk => chunk.CurrentChunk)
                        .Select(chunk => Encoding.UTF8.GetString(chunk.Payload)));

                    var chunkMessageReturn = new ChunkMessageReturn()
                    {
                        Body = fullMessage,
                        StatusCode = chunkMessageResponse.StatusCode
                    };

                    _logger.Information("Retorno da mensagem...");
                    tcs.SetResult(JsonConvert.SerializeObject(chunkMessageReturn));


                    _logger.Information("retira o correlationId do buffer...");
                    _chunksBuffer.Remove(correlationId);
                }
            };

            _logger.Information("Inicia o consumo das mensagens...");
            _channel.BasicConsume(queue: responseQueueName, autoAck: true, consumer: consumer);
            return tcs;
        }

        private async Task<string> SendMessageInChunk<T>(T request, string correlationId, byte[] messageBytes, string responseQueueName, TaskCompletionSource<string> tcs)
        {
            _logger.Information("Definição dos chunks...");
            int chunkSize = 128;
            int totalChunks = (int)Math.Ceiling((double)messageBytes.Length / chunkSize);

            _logger.Information("enviando mensagem em chunks...");
            try
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var chunk = messageBytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
                    var properties = _channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.CorrelationId = correlationId;
                    properties.ReplyTo = responseQueueName;

                    var chunkMessage = new ChunkMessageRequest
                    {
                        Payload = chunk,
                        TotalChunks = totalChunks,
                        CurrentChunk = i + 1,
                        RequestType = DetermineRequestType(request)
                    };

                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: _configuration["RabbitMqSettings:RequestQueueName"],
                        basicProperties: properties,
                        body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chunkMessage))
                    );
                }

                _logger.Information("esperando resposta...");

                var timeout = TimeSpan.FromSeconds(15);

                using (var cts = new CancellationTokenSource(timeout))
                {
                    try
                    {
                        return await Task.Run(() => tcs.Task, cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.Error("Tempo de espera excedido.");

                        var chunkMessageReturn = new ChunkMessageReturn
                        {
                            Body = "Tempo de espera excedido.",
                            StatusCode = 500
                        };
                        return JsonConvert.SerializeObject(chunkMessageReturn);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error($"Erro ao enviar mensagem ou ao processar a resposta: {ex.Message}");

                var chunkMessageReturn = new ChunkMessageReturn
                {
                    Body = JsonConvert.SerializeObject(ex.Message),
                    StatusCode = 500
                };
                return JsonConvert.SerializeObject(chunkMessageReturn);
            }
            finally
            {
                _logger.Information("retira a fila de resposta...");
                _channel.QueueDelete(responseQueueName);
            }
        }

        private ChunkMessageResponse? DeserializeResponseMessage(string message)
        {
            try
            {
                return JsonConvert.DeserializeObject<ChunkMessageResponse>(message);
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, $"Erro ao desserializar a mensagem: {ex.Message}");
                return null;
            }
        }

        private RequestTypeEnum DetermineRequestType<T>(T request)
        {
            switch (request)
            {
                case CreateClientRequest _:
                    return RequestTypeEnum.CreateClientRequest;

                case GetClientByIdRequest _:
                    return RequestTypeEnum.GetClientByIdRequest;

                case GetClientsRequest _:
                    return RequestTypeEnum.GetClientsRequest;

                case CreateProductRequest _:
                    return RequestTypeEnum.CreateProductRequest;

                case CreateOrderRequest _:
                    return RequestTypeEnum.CreateOrderRequest;

                case UpdateClientRequest _:
                    return RequestTypeEnum.UpdateClientRequest;

                case DeleteClientRequest _:
                    return RequestTypeEnum.DeleteClientRequest;

                case GetProductByIdRequest _:
                    return RequestTypeEnum.GetProductByIdRequest;

                case GetProductsRequest _:
                    return RequestTypeEnum.GetProductsRequest;

                case UpdateProductRequest _:
                    return RequestTypeEnum.UpdateProductRequest;
                
                case DeleteProductRequest _:
                    return RequestTypeEnum.DeleteProductRequest;
                
                case GetOrderValueRequest _:
                    return RequestTypeEnum.GetOrderValueRequest;
                
                case GetQtOrderByClientRequest _:
                    return RequestTypeEnum.GetQtOrderByClientRequest;
                
                case GetOrdersRequest _:
                    return RequestTypeEnum.GetOrdersRequest;
                
                case UpdateOrderRequest _:
                    return RequestTypeEnum.UpdateOrderRequest;
                
                case GetOrderByIdRequest _:
                    return RequestTypeEnum.GetOrderByIdRequest;
                
                case GetListOrderByClientRequest _:
                    return RequestTypeEnum.GetListOrderByClientRequest;
                
                case DeleteOrderRequest _:
                    return RequestTypeEnum.DeleteOrderRequest;
                
                default:
                    throw new ArgumentException("Tipo de requisição desconhecido", nameof(request));
            }
        }
    }
}

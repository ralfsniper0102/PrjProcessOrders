using Newtonsoft.Json;
using ProjProcessOrders.UseCase.Enums;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;
using ProjProcessOrders.UseCase.UseCases.CreateProduct;
using ProjProcessOrders.WebAPI.Infrastructure.Messaging.DTOs;
using RabbitMQ.Client;
using System.Text;

namespace ProjProcessOrders.WebAPI.Infrastructure.Messaging
{
    public class RabbitMqServiceWebAPI
    {
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public RabbitMqServiceWebAPI(IModel channel, IConfiguration configuration)
        {
            _channel = channel;
            _configuration = configuration;

            _channel.QueueDeclare(queue: _configuration["RabbitMqSettings:QueueName"],
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessageInChunks<T>(T request, string correlationId)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);
            var messageBytes = Encoding.UTF8.GetBytes(serializedRequest);

            int chunkSize = 128; 
            int totalChunks = (int)Math.Ceiling((double)messageBytes.Length / chunkSize);

            try
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var chunk = messageBytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
                    var properties = _channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.CorrelationId = correlationId;

                    var chunkMessage = new ChunkMessage
                    {
                        Payload = chunk,
                        TotalChunks = totalChunks,
                        CurrentChunk = i + 1,
                        RequestType = DetermineRequestType(request)
                    };

                    _channel.BasicPublish(exchange: "", routingKey: _configuration["RabbitMqSettings:QueueName"], basicProperties: properties, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chunkMessage)));
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem ou ao processar a resposta: {ex.Message}");
                throw;
            }
        }

        private RequestTypeEnum DetermineRequestType<T>(T request)
        {
            return request switch
            {
                CreateClientRequest _ => RequestTypeEnum.CreateClientRequest,
                CreateProductRequest _ => RequestTypeEnum.CreateProductRequest,
                CreateOrderRequest _ => RequestTypeEnum.CreateOrderRequest,
                _ => throw new ArgumentException("Tipo de requisição desconhecido", nameof(request))
            };
        }
    }

}

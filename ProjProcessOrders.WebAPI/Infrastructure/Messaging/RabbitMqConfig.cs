using RabbitMQ.Client;

namespace ProjProcessOrders.WebAPI.Infrastructure.Messaging
{
    public static class RabbitMqConfig
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var factory = new ConnectionFactory() { HostName = configuration["RabbitMqSettings:HostName"] }; 
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: configuration["RabbitMqSettings:QueueName"], durable: true, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton(channel);
            services.AddSingleton<RabbitMqServiceWebAPI>();
        }
    }
}

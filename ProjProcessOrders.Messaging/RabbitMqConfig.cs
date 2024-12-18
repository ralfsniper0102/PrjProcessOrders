using RabbitMQ.Client;

namespace ProjProcessOrders.Messaging
{
    public class RabbitMqConfig
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqConfig(string hostname, string queueName, int port)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                Port = port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public IModel GetChannel()
        {
            return _channel;
        }
    }
}

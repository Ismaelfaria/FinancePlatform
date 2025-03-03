using FinancePlatform.API.Application.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text;

namespace FinancePlatform.API.Infrastructure.Messaging
{
    public class RabbitMqMessageProcessor : IMessageProcessor
    {
        private readonly RabbitMqConnectionFactory _connectionFactory;
        private IModel _channel;

        public RabbitMqMessageProcessor(RabbitMqConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public bool PublishMessage(string exchange, string routingKey, string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                _channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                _channel.BasicPublish(exchange, routingKey, null, body);

                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                    channel.BasicPublish(exchange, routingKey, null, body);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
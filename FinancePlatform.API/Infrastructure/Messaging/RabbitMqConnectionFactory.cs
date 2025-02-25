using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace FinancePlatform.API.Infrastructure.Messaging
{
    public class RabbitMqConnectionFactory
    {
        private readonly RabbitMqSettings _rabbitMqSettings;

        public RabbitMqConnectionFactory(RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password,
                VirtualHost = _rabbitMqSettings.VirtualHost
            };

            return factory.CreateConnection();
        }
    }
}

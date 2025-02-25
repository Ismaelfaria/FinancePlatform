using FinancePlatform.API.Infrastructure.Messaging;

namespace FinancePlatform.API.Application.Messaging
{
    public class MessageService
    {
        private readonly RabbitMqMessageProcessor _messageProcessor;

        public MessageService(RabbitMqMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        public bool PublishMessageToQueue(string exchange, string routingKey, string message)
        {
            
            return _messageProcessor.PublishMessage(exchange, routingKey, message);
        }
    }
}

namespace FinancePlatform.API.Application.Interfaces.Messaging
{
    public interface IMessageProcessor
    {
        public bool PublishMessage(string exchange, string routingKey, string message);
    }
}

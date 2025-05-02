namespace FinancePlatform.API.Application.Interfaces.Messaging
{
    public interface IMessageProcessor
    {
        bool PublishMessage(string exchange, string routingKey, string message);
    }
}

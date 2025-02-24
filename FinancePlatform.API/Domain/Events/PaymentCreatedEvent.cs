namespace FinancePlatform.API.Domain.Events
{
    public class PaymentCreatedEvent
    {
        public Guid PaymentId { get; }
        public Guid AccountId { get; }
        public decimal Amount { get; }
        public DateTime CreatedAt { get; }

        public PaymentCreatedEvent(Guid paymentId, Guid accountId, decimal amount, DateTime createdAt)
        {
            PaymentId = paymentId;
            AccountId = accountId;
            Amount = amount;
            CreatedAt = createdAt;
        }
    }
}

namespace FinancePlatform.API.Domain.Events
{
    public class ReconciliationCompletedEvent
    {
        public Guid ReconciliationId { get; }
        public Guid PaymentId { get; }
        public bool IsSuccessful { get; }
        public DateTime ProcessedAt { get; }

        public ReconciliationCompletedEvent(Guid reconciliationId, Guid paymentId, bool isSuccessful, DateTime processedAt)
        {
            ReconciliationId = reconciliationId;
            PaymentId = paymentId;
            IsSuccessful = isSuccessful;
            ProcessedAt = processedAt;
        }
    }
}

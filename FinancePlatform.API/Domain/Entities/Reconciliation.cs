namespace FinancePlatform.API.Domain.Entities
{
    public class Reconciliation
    {
        public Guid Id { get; private set; }
        public Guid PaymentId { get; private set; }
        public DateTime ProcessedAt { get; private set; }
        public bool IsSuccessful { get; private set; }

        private Reconciliation() { }

        public Reconciliation(Guid paymentId, bool isSuccessful)
        {
            Id = Guid.NewGuid();
            PaymentId = paymentId;
            ProcessedAt = DateTime.UtcNow;
            IsSuccessful = isSuccessful;
        }
    }
}

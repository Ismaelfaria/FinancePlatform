using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public PaymentStatus Status { get; private set; }

        private Payment(){}

        public Payment(Guid accountId, decimal amount)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
        }
        public void Approve()
        {
            Status = PaymentStatus.Processed;
        }
        public void Reject()
        {
            Status = PaymentStatus.Rejected;
        }
    }
}

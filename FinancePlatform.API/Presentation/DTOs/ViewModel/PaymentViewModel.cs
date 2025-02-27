using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Presentation.DTOs.ViewModel
{
    public class PaymentViewModel
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public PaymentStatus Status { get; private set; }
    }
}

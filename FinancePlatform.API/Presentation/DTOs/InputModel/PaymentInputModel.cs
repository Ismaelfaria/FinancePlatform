using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Presentation.DTOs.InputModel
{
    public class PaymentInputModel
    {
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
    }
}

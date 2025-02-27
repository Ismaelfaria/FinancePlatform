namespace FinancePlatform.API.Presentation.DTOs.ViewModel
{
    public class ReconciliationViewModel
    {
        public Guid Id { get; private set; }
        public Guid PaymentId { get; private set; }
        public DateTime ProcessedAt { get; private set; }
        public bool IsSuccessful { get; private set; }
    }
}

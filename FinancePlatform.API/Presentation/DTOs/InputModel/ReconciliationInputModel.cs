namespace FinancePlatform.API.Presentation.DTOs.InputModel
{
    public class ReconciliationInputModel
    {
        public Guid PaymentId { get; private set; }
        public DateTime ProcessedAt { get; private set; }
        public bool IsSuccessful { get; private set; }
    }
}

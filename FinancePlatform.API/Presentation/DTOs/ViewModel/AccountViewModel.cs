namespace FinancePlatform.API.Presentation.DTOs.ViewModel
{
    public class AccountViewModel
    {
        public Guid Id { get; private set; }
        public string HolderName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
    }
}

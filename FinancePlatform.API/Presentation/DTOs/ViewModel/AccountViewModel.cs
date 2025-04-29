namespace FinancePlatform.API.Presentation.DTOs.ViewModel
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public string HolderName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}

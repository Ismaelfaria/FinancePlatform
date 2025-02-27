namespace FinancePlatform.API.Presentation.DTOs.InputModel
{
    public class AccountInputModel
    {
        public string HolderName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
    }
}

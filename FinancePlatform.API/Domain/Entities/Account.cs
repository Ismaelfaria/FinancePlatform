namespace FinancePlatform.API.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string HolderName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public Account(){}

        public Account(string holderName, string accountNumber, decimal initialBalance)
        {
            Id = Guid.NewGuid();
            HolderName = holderName;
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public void Credit(decimal amount)
        { 
            Balance += amount;
        }

        public bool Debit(decimal amount)
        {
            if (amount > Balance) return false;
            Balance -= amount;
            return true;
        }
    }
}

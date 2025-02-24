namespace FinancePlatform.API.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string HolderName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }

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

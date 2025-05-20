using FinancePlatform.API.Domain.ValueObjects;

namespace FinancePlatform.API.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string HolderName { get; private set; }          
        public int AccountNumber { get; private set; }              
        public string BranchCode { get; private set; }             
        public string BankCode { get; private set; }               
        public string DocumentNumber { get; private set; }        
        public DateTime CreatedAt { get; private set; }           
        public decimal Balance { get; private set; }             
        public bool IsActive { get; private set; }                 
        //public AccountType Type { get; private set; }            

        public Account(string holderName, string documentNumber, string branchCode, string bankCode, /*AccountType type*/ decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(holderName))
                throw new ArgumentException("Holder name is required.");
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("Document number is required.");

            Id = Guid.NewGuid();
            HolderName = holderName;
            DocumentNumber = DocumentNumberFormat(documentNumber);
            BranchCode = branchCode;
            BankCode = bankCode;
            AccountNumber = GenerateRandom6DigitNumber();
            //Type = type;
            Balance = initialBalance;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public Account(){
            Id = Guid.NewGuid();
        }

        public Account(string holderName, string accountNumber, decimal initialBalance)
        {
            Id = Guid.NewGuid();
            AccountNumber = GenerateRandom6DigitNumber();
            HolderName = holderName;
            Balance = initialBalance;
        }

        private string DocumentNumberFormat(string numberDocument)
        {
            var documentNumber = new DocumentNumber();

            return documentNumber.FormatCPForCNPJ(numberDocument);
        }

        public int GenerateRandom6DigitNumber()
        {
            Random random = new Random();
            return random.Next(100000, 1000000);
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

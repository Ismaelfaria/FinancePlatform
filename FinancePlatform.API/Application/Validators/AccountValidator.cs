using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Validators
{
    public class AccountValidator
    {
        public ValidationResult ValidateDeposit(decimal amount)
        {
            if (amount < 1)
                return new ValidationResult(false, "O valor do depósito deve ser maior que zero.");

            return new ValidationResult(true, null);
        }

        public ValidationResult ValidateWithdraw(decimal amount, decimal balance)
        {
            if (amount < 1)
                return new ValidationResult(false, "O valor do saque deve ser maior que zero.");

            if (amount > balance)
                return new ValidationResult(false, "Saldo insuficiente para saque.");

            return new ValidationResult(true, null);
        }
    }
}

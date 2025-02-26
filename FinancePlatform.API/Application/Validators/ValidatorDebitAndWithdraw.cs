
namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorDebitAndWithdraw
    {
        public bool ValidateDeposit(decimal amount)
        {
            if (amount < 1)
                return false;

            return true;
        }

        public bool ValidateWithdraw(decimal amount, decimal balance)
        {
            if (amount < 1) return false;

            if (amount > balance) return false;

            return true;
        }
    }
}

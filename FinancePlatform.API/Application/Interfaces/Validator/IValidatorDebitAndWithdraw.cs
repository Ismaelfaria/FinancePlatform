namespace FinancePlatform.API.Application.Interfaces.Validator
{
    public interface IValidatorDebitAndWithdraw
    {
        public bool ValidateDeposit(decimal amount);
        public bool ValidateWithdraw(decimal amount, decimal balance);
    }
}

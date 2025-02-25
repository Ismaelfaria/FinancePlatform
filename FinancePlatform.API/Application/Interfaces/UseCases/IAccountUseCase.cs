namespace FinancePlatform.API.Application.Interfaces.UseCases
{
    public interface IAccountUseCase
    {
        public Task<bool> Deposit(Guid accountId, decimal amount);
        public Task<decimal> FindBalance(Guid accountId);
        public Task<bool> Withdraw(Guid accountId, decimal amount);
    }
}

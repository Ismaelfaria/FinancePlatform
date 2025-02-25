using FinancePlatform.API.Application.Interfaces.Repositories;

namespace FinancePlatform.API.Application.UseCases.Account
{
    public class FindBalanceUseCase
    {
        private readonly IAccountRepository _accountRepository;

        public FindBalanceUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<decimal> ExecuteAsync(Guid accountId)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            return account?.Balance ?? 0;
        }
    }
}

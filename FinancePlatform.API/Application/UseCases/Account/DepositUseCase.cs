using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Validators;

namespace FinancePlatform.API.Application.UseCases.Account
{
    public class DepositUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountValidator _validator;

        public DepositUseCase(IAccountRepository accountRepository, AccountValidator validator)
        {
            _accountRepository = accountRepository;
            _validator = validator;
        }

        public async Task<bool> ExecuteAsync(Guid accountId, decimal amount)
        {
            var validationResult = _validator.ValidateDeposit(amount);
            if (!validationResult.IsValid) return false;

            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            account.Credit(amount);
            await _accountRepository.UpdateAsync(account);
            return true;
        }
    }
}

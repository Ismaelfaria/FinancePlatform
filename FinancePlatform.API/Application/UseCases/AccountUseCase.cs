using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Application.Interfaces.Validator;
using FinancePlatform.API.Application.Validators;

namespace FinancePlatform.API.Application.UseCases
{
    public class AccountUseCase : IAccountUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidatorDebitAndWithdraw _validator;

        public AccountUseCase(IAccountRepository accountRepository, 
                              IValidatorDebitAndWithdraw validator)
        {
            _accountRepository = accountRepository;
            _validator = validator;
        }

        public async Task<bool> Deposit(Guid accountId, decimal amount)
        {
            var validationResult = _validator.ValidateDeposit(amount);
            if (!validationResult) return false;

            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            account.Credit(amount);
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<decimal> FindBalance(Guid accountId)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            return account?.Balance ?? 0;
        }

        public async Task<bool> Withdraw(Guid accountId, decimal amount)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            var validationResult = _validator.ValidateWithdraw(amount, account.Balance);

            if (!validationResult) return false;

            if (!account.Debit(amount)) return false;

            await _accountRepository.UpdateAsync(account);
            return true;
        }
    }
}

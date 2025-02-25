using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Services;
using FinancePlatform.API.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace FinancePlatform.API.Application.UseCases.Account
{
    public class WithdrawUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountValidator _validator;

        public WithdrawUseCase(IAccountRepository accountRepository, AccountValidator validator)
        {
            _accountRepository = accountRepository;
            _validator = validator;
        }

        public async Task<bool> ExecuteAsync(Guid accountId, decimal amount)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            var validationResult = _validator.ValidateWithdraw(amount, account.Balance);

            if (!validationResult.IsValid) return false;

            if (!account.Debit(amount)) return false;

            await _accountRepository.UpdateAsync(account);
            return true;
        }
    }
}

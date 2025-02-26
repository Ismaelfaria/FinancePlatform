using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<Account> _validator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;

        public AccountService(IAccountRepository accountRepository, 
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IValidator<Account> validator)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
        }
        public async Task<List<Account>> FindAllAccountsAsync()
        {
            return await _accountRepository.FindAllAsync();
        }
        public async Task<Account> FindByIdAsync(Guid id)
        {
            return await _accountRepository.FindByIdAsync(id);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var validator = _validator.Validate(account);

            if (!validator.IsValid) return null;

            return await _accountRepository.AddAsync(account);
        }
        public async Task<Account> UpdateAsync(Guid accountId, Dictionary<string, object> updateRequest)
        {
            Account account = await _accountRepository.FindByIdAsync(accountId);

            if (account == null) return null;

            _entityUpdateStrategy.UpdateEntityFields(account, updateRequest);

            await _accountRepository.UpdateAsync(account);

            return account;
        }
        public async Task<bool> DeleteAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            _accountRepository.Delete(account);
            return true;
        }
    }
}

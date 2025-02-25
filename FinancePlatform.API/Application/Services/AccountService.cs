using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;

        public AccountService(IAccountRepository accountRepository, IEntityUpdateStrategy entityUpdateStrategy)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
        }
        public async Task<List<Account>> FindAllAccountsAsync()
        {
            return await _accountRepository.FindAllAsync();
        }
        public async Task<Account> FindByIdAsync(Guid id)
        {
            return await _accountRepository.FindByIdAsync(id);
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            if (account == null) return false;

            await _accountRepository.AddAsync(account);
            return true;
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

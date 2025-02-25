using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<List<Account>> FindAllAccountsAsync();
        public Task<Account> FindByIdAsync(Guid id);
        public Task<bool> CreateAccountAsync(Account account);
        public Task<bool> DeleteAccountAsync(Guid accountId);
        public Task<Account> UpdateAsync(Guid accountId, Account account);
    }
}

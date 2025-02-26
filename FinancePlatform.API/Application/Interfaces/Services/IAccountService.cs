using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<List<Account>> FindAllAccountsAsync();
        public Task<Account> FindByIdAsync(Guid id);
        public Task<Account> CreateAccountAsync(Account account);
        public Task<Account> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteAccountAsync(Guid accountId);
    }
}

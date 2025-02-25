using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        public Task<Account> FindByIdAsync(Guid id);
        public Task<List<Account>> FindAllAsync();
        public Task<Account> AddAsync(Account account);
        public Task<Account> UpdateAsync(Account account);
        public void Delete(Account account);
    }
}

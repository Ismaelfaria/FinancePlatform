using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlatform.API.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinanceDbContext _context;

        public AccountRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Account> FindByIdAsync(Guid id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Account>> FindAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            return account;
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            return account;
        }

        public bool Delete(Account account)
        {
            _context.Accounts.Remove(account);
            return true;
        }
    }
}

using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlatform.API.Infrastructure.Persistence.Repositories
{
    public class ReconciliationRepository : IReconciliationRepository
    {
        private readonly FinanceDbContext _context;

        public ReconciliationRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Reconciliation?> FindByIdAsync(Guid id)
        {
            return await _context.Reconciliations.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Reconciliation>?> FindAllAsync()
        {
            return await _context.Reconciliations.ToListAsync();
        }
        public async Task<Reconciliation?> AddAsync(Reconciliation reconciliation)
        {
            _context.Reconciliations.Add(reconciliation);
            await _context.SaveChangesAsync();
            return reconciliation;
        }

        public async Task<Reconciliation?> UpdateAsync(Reconciliation reconciliation)
        {
            _context.Reconciliations.Update(reconciliation);
            await _context.SaveChangesAsync();
            return reconciliation;
        }

        public bool Delete(Reconciliation reconciliation)
        {
            _context.Reconciliations.Remove(reconciliation);
            _context.SaveChanges(); 
            return true;
        }
    }
}

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

        public async Task<Reconciliation> FindByIdAsync(Guid id)
        {
            return await _context.Reconciliations.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Reconciliation>> FindAllAsync()
        {
            return await _context.Reconciliations.ToListAsync();
        }
        public void Add(Reconciliation reconciliation)
        {
            _context.Reconciliations.Add(reconciliation);
        }

        public void Update(Reconciliation reconciliation)
        {
            _context.Reconciliations.Update(reconciliation);
        }

        public void Delete(Reconciliation reconciliation)
        {
            _context.Reconciliations.Remove(reconciliation);
        }
    }
}

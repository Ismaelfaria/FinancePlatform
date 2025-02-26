using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface IReconciliationRepository
    {
        public Task<Reconciliation> FindByIdAsync(Guid id);
        public Task<List<Reconciliation>> FindAllAsync();
        public Task<Reconciliation> AddAsync(Reconciliation reconciliation);
        public Task<Reconciliation> UpdateAsync(Reconciliation reconciliation);
        public bool Delete(Reconciliation reconciliation);
    }
}

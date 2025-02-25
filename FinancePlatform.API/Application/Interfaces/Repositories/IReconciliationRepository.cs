using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface IReconciliationRepository
    {
        public Task<Reconciliation> FindByIdAsync(Guid id);
        public Task<List<Reconciliation>> FindAllAsync();
        public void Add(Reconciliation reconciliation);
        public void Update(Reconciliation reconciliation);
        public void Delete(Reconciliation reconciliation);
    }
}

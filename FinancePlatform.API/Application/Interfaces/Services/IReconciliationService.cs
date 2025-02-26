using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IReconciliationService
    {
        public Task<Reconciliation> CreateReconciliation(Reconciliation reconciliation);
        public Task<Reconciliation> GetReconciliationByIdAsync(Guid id);
        public Task<List<Reconciliation>> GetAllReconciliationsAsync();
        public Task<Reconciliation> UpdateReconciliationAsync(Guid reconciliationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteReconciliationAsync(Guid reconciliationId);
    }
}

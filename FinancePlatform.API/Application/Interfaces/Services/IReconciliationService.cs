using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IReconciliationService
    {
        public Task<Reconciliation?> CreateAsync(ReconciliationInputModel reconciliation);
        public Task<ReconciliationViewModel?> FindByIdAsync(Guid id);
        public Task<List<ReconciliationViewModel>?> FindAllAsync();
        public Task<Reconciliation?> UpdateAsync(Guid reconciliationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteAsync(Guid reconciliationId);
    }
}

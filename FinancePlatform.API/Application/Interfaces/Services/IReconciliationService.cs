﻿using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IReconciliationService
    {
        public Task<Reconciliation?> CreateReconciliation(ReconciliationInputModel reconciliation);
        public Task<ReconciliationViewModel?> FindReconciliationByIdAsync(Guid id);
        public Task<List<ReconciliationViewModel>?> FindAllReconciliationsAsync();
        public Task<Reconciliation?> UpdateReconciliationAsync(Guid reconciliationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteReconciliationAsync(Guid reconciliationId);
    }
}

using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.Services
{
    public class ReconciliationService
    {
        private readonly IReconciliationRepository _reconciliationRepository;
        private readonly IValidator<Reconciliation> _validator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;

        public ReconciliationService(IReconciliationRepository reconciliationRepository,
                                     IEntityUpdateStrategy entityUpdateStrategy,
                                     IValidator<Reconciliation> validator)
        {
            _reconciliationRepository = reconciliationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
        }

        public async Task<Reconciliation> CreateReconciliation(Reconciliation reconciliation)
        {
            var validator = _validator.Validate(reconciliation);

            if (!validator.IsValid) return null;

            return await _reconciliationRepository.AddAsync(reconciliation);
        }

        public async Task<Reconciliation> GetReconciliationByIdAsync(Guid id)
        {
            return await _reconciliationRepository.FindByIdAsync(id);
        }

        public async Task<List<Reconciliation>> GetAllReconciliationsAsync()
        {
            return await _reconciliationRepository.FindAllAsync();
        }

        public async Task<Reconciliation> UpdateReconciliationAsync(Guid reconciliationId, Dictionary<string, object> updateRequest)
        {
            var reconciliationResult = await _reconciliationRepository.FindByIdAsync(reconciliationId);
            if (reconciliationResult == null) return null;

            _entityUpdateStrategy.UpdateEntityFields(reconciliationResult, updateRequest);

            var reconciliationUpdate = await _reconciliationRepository.UpdateAsync(reconciliationResult);

            return reconciliationUpdate;
        }

        public async Task<bool> DeleteReconciliationAsync(Guid reconciliationId)
        {
            var reconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);
            if (reconciliation == null) return false;

            _reconciliationRepository.Delete(reconciliation);
            return true;
        }
    }
}
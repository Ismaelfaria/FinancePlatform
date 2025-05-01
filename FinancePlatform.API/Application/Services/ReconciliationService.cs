using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using Mapster;
using MapsterMapper;

namespace FinancePlatform.API.Application.Services
{
    public class ReconciliationService : IReconciliationService
    {
        private readonly IReconciliationRepository _reconciliationRepository;
        private readonly IValidator<Reconciliation> _validator;
        private readonly IValidator<Guid> _guidValidator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;

        public ReconciliationService(IReconciliationRepository reconciliationRepository,
                                     IEntityUpdateStrategy entityUpdateStrategy,
                                     IValidator<Guid> guidValidator,
                                     IValidator<Reconciliation> validator,
                                     IMapper mapper)
        {
            _reconciliationRepository = reconciliationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Reconciliation?> CreateReconciliation(ReconciliationInputModel model)
        {
            var reconciliation = model.Adapt<Reconciliation>();
            var validator = _validator.Validate(reconciliation);
            if (!validator.IsValid) return null;

            var createdReconciliation = await _reconciliationRepository.AddAsync(reconciliation);

            return createdReconciliation;
        }

        public async Task<ReconciliationViewModel?> FindReconciliationByIdAsync(Guid reconciliationId)
        {
            var validationResult = _guidValidator.Validate(reconciliationId);
            if (!validationResult.IsValid) return null;

            var existingReconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);
            if (existingReconciliation == null)
            {
                return null;
            }

            return _mapper.Map<ReconciliationViewModel>(existingReconciliation);
        }

        public async Task<List<ReconciliationViewModel>?> FindAllReconciliationsAsync()
        {
            var existingReconciliation = await _reconciliationRepository.FindAllAsync();
            if (existingReconciliation == null || existingReconciliation.Count == 0) return null;

            return _mapper.Map<List<ReconciliationViewModel>>(existingReconciliation);
        }

        public async Task<Reconciliation?> UpdateReconciliationAsync(Guid reconciliationId, Dictionary<string, object> updateRequest)
        {
            var validationResult = _guidValidator.Validate(reconciliationId);
            if (!validationResult.IsValid) return null;

            var reconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);
            if (reconciliation == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(reconciliation, updateRequest);
            if (isUpdateSuccessful)
            {
                await _reconciliationRepository.UpdateAsync(reconciliation);
            }
            return reconciliation;
        }

        public async Task<bool> DeleteReconciliationAsync(Guid reconciliationId)
        {
            var validationResult = _guidValidator.Validate(reconciliationId);
            if (!validationResult.IsValid) return false;

            var reconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);
            if (reconciliation == null) return false;

            _reconciliationRepository.Delete(reconciliation);

            return true;
        }
    }
}
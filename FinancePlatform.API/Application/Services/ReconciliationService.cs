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
        private readonly ICacheRepository _cacheRepository;
        private const string CACHE_COLLECTION_KEY = "_AllReconciliation";

        public ReconciliationService(IReconciliationRepository reconciliationRepository,
                                     IEntityUpdateStrategy entityUpdateStrategy,
                                     IValidator<Guid> guidValidator,
                                     IValidator<Reconciliation> validator,
                                     IMapper mapper,
                                     ICacheRepository cacheRepository)
        {
            _reconciliationRepository = reconciliationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<Reconciliation?> CreateAsync(ReconciliationInputModel model)
        {
            var reconciliation = model.Adapt<Reconciliation>();

            var validator = _validator.Validate(reconciliation);
            if (!validator.IsValid) return null;

            var createdReconciliation = await _reconciliationRepository.AddAsync(reconciliation);

            return createdReconciliation;
        }

        public async Task<ReconciliationViewModel?> FindByIdAsync(Guid reconciliationId)
        {
            var validationResult = _guidValidator.Validate(reconciliationId);
            if (!validationResult.IsValid) return null;

            var reconciliation = await _cacheRepository.GetValue<ReconciliationViewModel>(reconciliationId);

            if (reconciliation == null)
            {
                var existingReconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);
                if (existingReconciliation == null)
                {
                    return null;
                }

                var reconciliationViewModel = _mapper.Map<ReconciliationViewModel>(existingReconciliation);
                await _cacheRepository.SetValue(reconciliationId, reconciliationViewModel);
                return reconciliationViewModel;
            }

            return _mapper.Map<ReconciliationViewModel>(reconciliation);
        }

        public async Task<List<ReconciliationViewModel>?> FindAllAsync()
        {
            var reconciliations = await _cacheRepository.GetCollection<ReconciliationViewModel>(CACHE_COLLECTION_KEY);
            
            if (reconciliations == null || !reconciliations.Any())
            {
                var existingReconciliation = await _reconciliationRepository.FindAllAsync();
                if (existingReconciliation == null || existingReconciliation.Count == 0)
                {
                    return null;
                }

                var reconciliationViewModels = _mapper.Map<List<ReconciliationViewModel>>(existingReconciliation);
                await _cacheRepository.SetCollection(CACHE_COLLECTION_KEY, reconciliationViewModels);
                return reconciliationViewModels;
            }

            return _mapper.Map<List<ReconciliationViewModel>>(reconciliations);
        }

        public async Task<Reconciliation?> UpdateAsync(Guid reconciliationId, Dictionary<string, object> updateRequest)
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

        public async Task<bool> DeleteAsync(Guid reconciliationId)
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
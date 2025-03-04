using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Services.Cache;
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
        private readonly CacheService _cacheService;

        public ReconciliationService(IReconciliationRepository reconciliationRepository,
                                     IEntityUpdateStrategy entityUpdateStrategy,
                                     IValidator<Guid> guidValidator,
                                     IValidator<Reconciliation> validator,
                                     IMapper mapper,
                                     CacheService cacheService)
        {
            _reconciliationRepository = reconciliationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Reconciliation?> CreateReconciliation(ReconciliationInputModel model)
        {
            var reconciliation = model.Adapt<Reconciliation>();
            var validator = _validator.Validate(reconciliation);

            if (!validator.IsValid) return null;

            var createdReconciliation = await _reconciliationRepository.AddAsync(reconciliation);

            if (createdReconciliation != null)
            {
                string reconciliationCacheKey = $"reconciliation:{createdReconciliation.Id}";

                await _cacheService.SetAsync(reconciliationCacheKey, createdReconciliation);
            }

            return createdReconciliation;
        }

        public async Task<ReconciliationViewModel?> FindReconciliationByIdAsync(Guid reconciliationId)
        {
            var validationResult = _guidValidator.Validate(reconciliationId);

            if (!validationResult.IsValid) return null;

            string reconciliationCacheKey = $"reconciliation:{reconciliationId}";

            var cachedReconciliation = await _cacheService.GetAsync<Reconciliation>(reconciliationCacheKey);
            if (cachedReconciliation != null)
            {
                return _mapper.Map<ReconciliationViewModel>(cachedReconciliation);
            }

            var existingReconciliation = await _reconciliationRepository.FindByIdAsync(reconciliationId);

            if (existingReconciliation == null)
            {
                return null;
            }
            await _cacheService.SetAsync(reconciliationCacheKey, existingReconciliation);

            return _mapper.Map<ReconciliationViewModel>(existingReconciliation);
        }

        public async Task<List<ReconciliationViewModel>?> FindAllReconciliationsAsync()
        {
            string reconciliationListCacheKey = "reconciliations:list";

            var cachedReconciliations = await _cacheService.GetAsync<List<Reconciliation>>(reconciliationListCacheKey);

            if (cachedReconciliations != null)
            {
                return _mapper.Map<List<ReconciliationViewModel>>(cachedReconciliations);
            }

            var existingReconciliation = await _reconciliationRepository.FindAllAsync();
            
            if (existingReconciliation == null || existingReconciliation.Count == 0) return null;

            await _cacheService.SetAsync(reconciliationListCacheKey, existingReconciliation);

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

                string reconciliationCacheKey = $"reconciliation:{reconciliationId}";
                await _cacheService.UpdateCacheIfNeededAsync(reconciliationCacheKey, reconciliation);
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

            string accountCacheKey = $"reconciliation:{reconciliationId}";
            await _cacheService.RemoveFromCacheIfNeededAsync<Account>(accountCacheKey);
            
            return true;
        }
    }
}
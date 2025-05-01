using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using MapsterMapper;

namespace FinancePlatform.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;
        private readonly IValidator<Guid> _guidValidator;
        private readonly ICacheRepository _cacheRepository;
        private const string CACHE_COLLECTION_KEY = "_AllPayments";

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IMapper mapper,
                              IValidator<Guid> guidValidator,
                              ICacheRepository cacheRepository)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<PaymentViewModel?> FindByIdAsync(Guid paymentId)
        {
            var validationResult = _guidValidator.Validate(paymentId);
            if (!validationResult.IsValid) return null;

            var payment = await _cacheRepository.GetValue<PaymentViewModel>(paymentId);
            
            if (payment == null)
            {
                var existingPayment = await _paymentRepository.FindByIdAsync(paymentId);
                if (existingPayment == null) return null;

                var paymentViewModel = _mapper.Map<PaymentViewModel>(existingPayment);
                await _cacheRepository.SetValue(paymentId, paymentViewModel);
                return paymentViewModel;
            }

            return _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task<List<PaymentViewModel>?> FindAllAsync()
        {
            var notifications = await _cacheRepository.GetCollection<PaymentViewModel>(CACHE_COLLECTION_KEY);
            
            if (notifications == null || !notifications.Any())
            {
                var existingPayments = await _paymentRepository.FindAllAsync();
                if (existingPayments == null || existingPayments.Count == 0)
                {
                    return null;
                }

                var paymentsViewModels = _mapper.Map<List<PaymentViewModel>>(existingPayments);
                await _cacheRepository.SetCollection(CACHE_COLLECTION_KEY, paymentsViewModels);
                return paymentsViewModels;
            }

            return _mapper.Map<List<PaymentViewModel>>(notifications);
        }

        public async Task<Payment?> UpdateAsync(Guid paymentId, Dictionary<string, object> updateRequest)
        {
            var validationResult = _guidValidator.Validate(paymentId);

            if (!validationResult.IsValid) return null;

            var payment = await _paymentRepository.FindByIdAsync(paymentId);

            if (payment == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(payment, updateRequest);

            if (isUpdateSuccessful)
            {
                await _paymentRepository.UpdateAsync(payment);

            }
            return payment;
        }

        public async Task<bool> DeleteAsync(Guid paymentId)
        {
            var validationResult = _guidValidator.Validate(paymentId);

            if (!validationResult.IsValid) return false;

            var payment = await _paymentRepository.FindByIdAsync(paymentId);

            if (payment == null) return false;

            _paymentRepository.Delete(payment);

            return true;
        }
    }
}

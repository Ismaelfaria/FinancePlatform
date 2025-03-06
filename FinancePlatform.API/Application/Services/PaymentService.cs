using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Services.Cache;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using MapsterMapper;
using Microsoft.Identity.Client;
using System.Security.Principal;

namespace FinancePlatform.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;
        private readonly IValidator<Guid> _guidValidator;
        private readonly ICacheService _cacheService;

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IMapper mapper,
                              IValidator<Guid> guidValidator,
                              ICacheService cacheService)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaymentViewModel?> GetPaymentByIdAsync(Guid paymentId)
        {
            var validationResult = _guidValidator.Validate(paymentId);

            if (!validationResult.IsValid) return null;

            string paymentCacheKey = $"payment:{paymentId}";

            var cachedPayment = await _cacheService.GetAsync<Account>(paymentCacheKey);
            if (cachedPayment != null)
            {
                return _mapper.Map<PaymentViewModel>(cachedPayment);
            }

            var existingPayment = await _paymentRepository.FindByIdAsync(paymentId);
            if (existingPayment == null) return null;

            await _cacheService.SetAsync(paymentCacheKey, existingPayment);

            return _mapper.Map<PaymentViewModel>(existingPayment);
        }

        public async Task<List<PaymentViewModel>?> GetAllPaymentsAsync()
        {
            string paymentListCacheKey = "payment:list";

            var cachedPayments = await _cacheService.GetAsync<List<Account>>(paymentListCacheKey);
            if (cachedPayments != null)
            {
                return _mapper.Map<List<PaymentViewModel>>(cachedPayments);
            }

            var existingPayments = await _paymentRepository.FindAllAsync();
            if (existingPayments == null || existingPayments.Count == 0)
            {
                return null;
            }

            await _cacheService.SetAsync(paymentListCacheKey, existingPayments);
            return _mapper.Map<List<PaymentViewModel>>(existingPayments);
        }

        public async Task<Payment?> UpdatePaymentAsync(Guid paymentId, Dictionary<string, object> updateRequest)
        {
            var validationResult = _guidValidator.Validate(paymentId);

            if (!validationResult.IsValid) return null;

            var payment = await _paymentRepository.FindByIdAsync(paymentId);
            
            if (payment == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(payment, updateRequest);
            
            if (isUpdateSuccessful)
            {
                await _paymentRepository.UpdateAsync(payment);

                string paymentCacheKey = $"payment:{paymentId}";
                await _cacheService.UpdateCacheIfNeededAsync(paymentCacheKey, payment);
            }
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(Guid paymentId)
        {
            var validationResult = _guidValidator.Validate(paymentId);

            if (!validationResult.IsValid) return false;

            var payment = await _paymentRepository.FindByIdAsync(paymentId);
            
            if (payment == null) return false;

            _paymentRepository.Delete(payment);

            string accountCacheKey = $"payment:{paymentId}";
            await _cacheService.RemoveFromCacheIfNeededAsync<Account>(accountCacheKey);
            
            return true;
        }
    }
}

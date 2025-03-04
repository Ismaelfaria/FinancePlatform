using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Services.Cache;
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
        private readonly IValidator<Payment> _validator;
        private readonly IMapper _mapper;
        private readonly CacheService _cacheService;

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IMapper mapper,
                              CacheService cacheService,
                              IValidator<Payment> validator)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaymentViewModel?> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.FindByIdAsync(id);

            return _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task<List<PaymentViewModel>?> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.FindAllAsync();

            return _mapper.Map<List<PaymentViewModel>>(payments);
        }

        public async Task<Payment?> UpdatePaymentAsync(Guid paymentId, Dictionary<string, object> updateRequest)
        {
            var paymentResult = await _paymentRepository.FindByIdAsync(paymentId);
            if (paymentResult == null) return null;

            _entityUpdateStrategy.UpdateEntityFields(paymentResult, updateRequest);

            var paymentUpdate = await _paymentRepository.UpdateAsync(paymentResult);

            return paymentUpdate;
        }

        public async Task<bool> DeletePaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.FindByIdAsync(paymentId);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            return true;
        }
    }
}

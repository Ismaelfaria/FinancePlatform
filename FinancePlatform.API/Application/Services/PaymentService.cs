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

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IMapper mapper,
                              IValidator<Guid> guidValidator)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _mapper = mapper;
        }

        public async Task<PaymentViewModel?> GetPaymentByIdAsync(Guid paymentId)
        {
            var validationResult = _guidValidator.Validate(paymentId);
            if (!validationResult.IsValid) return null;

            var existingPayment = await _paymentRepository.FindByIdAsync(paymentId);
            if (existingPayment == null) return null;

            return _mapper.Map<PaymentViewModel>(existingPayment);
        }

        public async Task<List<PaymentViewModel>?> GetAllPaymentsAsync()
        {
            var existingPayments = await _paymentRepository.FindAllAsync();
            if (existingPayments == null || existingPayments.Count == 0)
            {
                return null;
            }

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

            return true;
        }
    }
}

using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using MapsterMapper;

namespace FinancePlatform.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _mapper = mapper;
        }

        public async Task<PaymentViewModel> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.FindByIdAsync(id);

            return _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task<List<PaymentViewModel>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.FindAllAsync();

            return _mapper.Map<List<PaymentViewModel>>(payments);
        }

        public async Task<Payment> UpdatePaymentAsync(Guid paymentId, Dictionary<string, object> updateRequest)
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

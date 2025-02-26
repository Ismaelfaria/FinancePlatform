using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;

        public PaymentService(IPaymentRepository paymentRepository,
                              IEntityUpdateStrategy entityUpdateStrategy)
        {
            _paymentRepository = paymentRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _paymentRepository.FindByIdAsync(id);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.FindAllAsync();
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

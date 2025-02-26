using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.UseCases
{
    public interface IPaymentUseCase
    {
        public Task<Payment> generatePayment(Payment payment);
    }
}

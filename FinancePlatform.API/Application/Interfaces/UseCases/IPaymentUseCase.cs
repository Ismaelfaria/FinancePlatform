using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;

namespace FinancePlatform.API.Application.Interfaces.UseCases
{
    public interface IPaymentUseCase
    {
        public Task<Payment> generatePayment(PaymentInputModel payment);
    }
}

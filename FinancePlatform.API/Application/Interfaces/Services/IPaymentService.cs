using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        public Task<PaymentViewModel?> FindByIdAsync(Guid id);
        public Task<List<PaymentViewModel>?> FindAllAsync();
        public Task<Payment?> UpdatePaymentAsync(Guid paymentId, Dictionary<string, object> updateRequest);
        public Task<bool> DeletePaymentAsync(Guid paymentId);
    }
}

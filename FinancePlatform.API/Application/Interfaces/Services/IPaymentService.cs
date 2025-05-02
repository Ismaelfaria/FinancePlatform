using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentViewModel?> FindByIdAsync(Guid id);
        Task<List<PaymentViewModel>?> FindAllAsync();
        Task<Payment?> UpdateAsync(Guid paymentId, Dictionary<string, object> updateRequest);
        Task<bool> DeleteAsync(Guid paymentId);
    }
}

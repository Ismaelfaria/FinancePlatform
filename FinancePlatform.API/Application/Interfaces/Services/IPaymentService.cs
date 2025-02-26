using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        public Task<Payment> GetPaymentByIdAsync(Guid id);
        public Task<List<Payment>> GetAllPaymentsAsync();
        public Task<Payment> UpdatePaymentAsync(Guid paymentId, Dictionary<string, object> updateRequest);
        public Task<bool> DeletePaymentAsync(Guid paymentId);
    }
}

using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        public Task<Payment> FindByIdAsync(Guid id);
        public Task<List<Payment>> FindAllAsync();
        public Task<Payment> AddAsync(Payment payment);
        public Task<Payment> UpdateAsync(Payment payment);
        public bool Delete(Payment payment);
    }
}

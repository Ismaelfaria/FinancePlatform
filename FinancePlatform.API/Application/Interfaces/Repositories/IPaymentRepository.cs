using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        public Task<Payment> FindByIdAsync(Guid id);
        public Task<List<Payment>> FindAllAsync();
        public void Add(Payment payment);
        public void Update(Payment payment);
        public void Delete(Payment payment);
    }
}

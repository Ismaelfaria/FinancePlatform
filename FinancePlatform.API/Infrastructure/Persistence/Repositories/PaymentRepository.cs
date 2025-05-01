using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlatform.API.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FinanceDbContext _context;

        public PaymentRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> FindByIdAsync(Guid id)
        {
            return await _context.Payments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Payment>> FindAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public bool Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
            _context.SaveChanges();
            return true;
        }
    }
}

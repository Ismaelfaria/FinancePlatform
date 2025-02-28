using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Infrastructure.Persistence.Repositories;

namespace FinancePlatform.API.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceDbContext _context;

        
        public IAccountRepository Accounts { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IReconciliationRepository Reconciliations { get; private set; }
        public INotificationRepository Notifications { get; private set; }

        public UnitOfWork(FinanceDbContext context)
        {
            _context = context;

            Accounts = new AccountRepository(_context);
            Payments = new PaymentRepository(_context);
            Reconciliations = new ReconciliationRepository(_context);
            Notifications = new NotificationRepository(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        
        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    entry.Reload(); 
                }
            }
        }
    }
}

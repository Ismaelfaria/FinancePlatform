using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlatform.API.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FinanceDbContext _context;

        public NotificationRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> FindByIdAsync(Guid id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Notification>?> FindAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification?> AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public bool Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
            _context.SaveChanges();
            return true;
        }
    }
}

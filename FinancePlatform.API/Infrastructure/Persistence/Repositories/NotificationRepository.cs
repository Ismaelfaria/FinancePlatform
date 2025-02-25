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

        public async Task<Notification> FindByIdAsync(Guid id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Notification>> FindAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }
        public async Task<Notification> Add(Notification notification)
        {
            _context.Notifications.Add(notification);
            return notification;
        }

        public async Task<Notification> Update(Notification notification)
        {
            _context.Notifications.Update(notification);
            return notification;
        }

        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }
    }
}

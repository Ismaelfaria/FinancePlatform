using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Infrastructure.Persistence.Repositories;

namespace FinancePlatform.API.Application.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;

        public NotificationService(INotificationRepository notificationRepository, IEntityUpdateStrategy entityUpdateStrategy)
        {
            _notificationRepository = notificationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid idNotification)
        {
            return await _notificationRepository.FindByIdAsync(idNotification);
        }
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.FindAllAsync();
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await _notificationRepository.Add(notification);
            return notification;
        }
        public async Task<Notification> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest)
        {
            Notification notification = await _notificationRepository.FindByIdAsync(notificationId);
            if (notification == null) return null;

            _entityUpdateStrategy.UpdateEntityFields(notification, updateRequest);

            await _notificationRepository.Update(notification);

            return notification;
        }
        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.FindByIdAsync(id);
            if (notification == null) return false;

            _notificationRepository.Delete(notification);
            return true;
        }
    }
}


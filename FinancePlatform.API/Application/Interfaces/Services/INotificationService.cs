using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<Notification?> GetNotificationByIdAsync(Guid idNotification);
        public Task<List<Notification>> GetAllNotificationsAsync();
        public Task<Notification> CreateNotificationAsync(NotificationInputModel model);
        public Task<Notification> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteNotificationAsync(Guid id);
    }
}

using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<NotificationViewModel?> GetNotificationByIdAsync(Guid idNotification);
        public Task<List<NotificationViewModel>?> GetAllNotificationsAsync();
        public Task<Notification?> CreateNotificationAsync(NotificationInputModel model);
        public Task<Notification?> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteNotificationAsync(Guid id);
    }
}

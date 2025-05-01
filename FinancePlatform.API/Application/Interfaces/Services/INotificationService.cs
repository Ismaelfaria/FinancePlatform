using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<NotificationViewModel?> FindByIdAsync(Guid idNotification);
        public Task<List<NotificationViewModel>?> FindAllAsync();
        public Task<Notification?> CreateAsync(NotificationInputModel model);
        public Task<Notification?> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest);
        public Task<bool> DeleteAsync(Guid id);
    }
}

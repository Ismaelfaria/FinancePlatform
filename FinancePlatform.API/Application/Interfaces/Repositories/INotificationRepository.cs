using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<Notification> FindByIdAsync(Guid id);
        public Task<List<Notification>> FindAllAsync();
        public Task<Notification> Add(Notification notification);
        public Task<Notification> Update(Notification notification);
        public bool Delete(Notification notification);
    }
}

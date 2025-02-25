using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<Notification> FindByIdAsync(Guid id);
        public Task<List<Notification>> FindAllAsync();
        public void Add(Notification notification);
        public void Update(Notification notification);
        public void Delete(Notification notification);
    }
}

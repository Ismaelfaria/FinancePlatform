using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using Mapster;
using MapsterMapper;
using System.Security.Principal;

namespace FinancePlatform.API.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<Notification> _validator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, 
                                   IEntityUpdateStrategy entityUpdateStrategy,
                                   IValidator<Notification> validator,
                                   IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<NotificationViewModel?> GetNotificationByIdAsync(Guid idNotification)
        {
            var notification = await _notificationRepository.FindByIdAsync(idNotification);

            return _mapper.Map<NotificationViewModel>(notification);
        }

        public async Task<List<NotificationViewModel>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.FindAllAsync();

            return _mapper.Map<List<NotificationViewModel>>(notifications);
        }

        public async Task<Notification> CreateNotificationAsync(NotificationInputModel model)
        {
            var notification = model.Adapt<Notification>();
            var validator = _validator.Validate(notification);

            if (!validator.IsValid) return null;

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


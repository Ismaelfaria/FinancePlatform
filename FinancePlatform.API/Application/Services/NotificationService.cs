using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using Mapster;
using MapsterMapper;

namespace FinancePlatform.API.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<Notification> _validator;
        private readonly IValidator<Guid> _guidValidator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, 
                                   IEntityUpdateStrategy entityUpdateStrategy,
                                   IValidator<Notification> validator,
                                   IValidator<Guid> guidValidator,
                                   IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<NotificationViewModel?> FindNotificationByIdAsync(Guid notificationId)
        {
            var validationResult = _guidValidator.Validate(notificationId);
            if (!validationResult.IsValid) return null;

            var existingNotification = await _notificationRepository.FindByIdAsync(notificationId);
            if (existingNotification == null) return null;

            return _mapper.Map<NotificationViewModel>(existingNotification);
        }

        public async Task<List<NotificationViewModel>?> FindAllNotificationsAsync()
        {
            var existingNotifications = await _notificationRepository.FindAllAsync();
            
            if (existingNotifications == null || existingNotifications.Count == 0)
            {
                return null;
            }

            return _mapper.Map<List<NotificationViewModel>>(existingNotifications);
        }

        public async Task<Notification?> CreateNotificationAsync(NotificationInputModel model)
        {
            var notification = model.Adapt<Notification>();
            var validator = _validator.Validate(notification);

            if (!validator.IsValid) return null;

            var createdNotification = await _notificationRepository.Add(notification);

            return notification;
        }

        public async Task<Notification?> UpdateAsync(Guid notificationId, Dictionary<string, object> updateRequest)
        {
            var validationResult = _guidValidator.Validate(notificationId);

            if (!validationResult.IsValid) return null;

            var notification = await _notificationRepository.FindByIdAsync(notificationId);
            
            if (notification == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(notification, updateRequest);

            if (isUpdateSuccessful)
            {
                await _notificationRepository.Update(notification);
            }
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(Guid notificationId)
        {
            var validationResult = _guidValidator.Validate(notificationId);

            if (!validationResult.IsValid) return false;

            var notification = await _notificationRepository.FindByIdAsync(notificationId);
            
            if (notification == null) return false;

            _notificationRepository.Delete(notification);

            return true;
        }
    }
}


using FinancePlatform.API.Application.Interfaces.Cache;
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
        private readonly ICacheRepository _cacheRepository;
        private const string CACHE_COLLECTION_KEY = "_AllNotifications";

        public NotificationService(INotificationRepository notificationRepository,
                                   IEntityUpdateStrategy entityUpdateStrategy,
                                   IValidator<Notification> validator,
                                   IValidator<Guid> guidValidator,
                                   IMapper mapper,
                                   ICacheRepository cacheRepository)
        {
            _notificationRepository = notificationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<NotificationViewModel?> FindByIdAsync(Guid notificationId)
        {
            var validationResult = _guidValidator.Validate(notificationId);
            if (!validationResult.IsValid) return null;

            var notification = await _cacheRepository.GetValue<NotificationViewModel>(notificationId);
            if (notification == null)
            {
                var existingNotification = await _notificationRepository.FindByIdAsync(notificationId);
                if (existingNotification == null) return null;

                var notificationViewModel = _mapper.Map<NotificationViewModel>(existingNotification);
                await _cacheRepository.SetValue(notificationId, notificationViewModel);
                return notificationViewModel;
            }

            return _mapper.Map<NotificationViewModel>(notification);
        }

        public async Task<List<NotificationViewModel>?> FindAllAsync()
        {
            var notifications = await _cacheRepository.GetCollection<NotificationViewModel>(CACHE_COLLECTION_KEY);

            if (notifications == null || !notifications.Any())
            {
                var existingNotifications = await _notificationRepository.FindAllAsync();
                if (existingNotifications == null || !existingNotifications.Any())
                {
                    return null;
                }

                var notificationViewModels = _mapper.Map<List<NotificationViewModel>>(existingNotifications);
                await _cacheRepository.SetCollection(CACHE_COLLECTION_KEY, notificationViewModels);
                return notificationViewModels;
            }

            return _mapper.Map<List<NotificationViewModel>>(notifications);
        }

        public async Task<Notification?> AddAsync(NotificationInputModel model)
        {
            var notification = model.Adapt<Notification>();
            var validator = _validator.Validate(notification);

            if (!validator.IsValid) return null;

            var createdNotification = await _notificationRepository.AddAsync(notification);

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
                await _notificationRepository.UpdateAsync(notification);
            }
            return notification;
        }

        public async Task<bool> DeleteAsync(Guid notificationId)
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


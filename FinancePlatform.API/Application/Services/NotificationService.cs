using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Services.Cache;
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
        private readonly ICacheService _cacheService;

        public NotificationService(INotificationRepository notificationRepository, 
                                   IEntityUpdateStrategy entityUpdateStrategy,
                                   IValidator<Notification> validator,
                                   IValidator<Guid> guidValidator,
                                   IMapper mapper,
                                   ICacheService cacheService)
        {
            _notificationRepository = notificationRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _guidValidator = guidValidator;
            _validator = validator;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<NotificationViewModel?> FindNotificationByIdAsync(Guid notificationId)
        {
            var validationResult = _guidValidator.Validate(notificationId);

            if (!validationResult.IsValid) return null;

            string notificationCacheKey = $"account:{notificationId}";

            var cachedNotification = await _cacheService.GetAsync<Account>(notificationCacheKey);
            
            if (cachedNotification != null)
            {
                return _mapper.Map<NotificationViewModel>(cachedNotification);
            }

            var existingNotification = await _notificationRepository.FindByIdAsync(notificationId);

            if (existingNotification == null) return null;

            await _cacheService.SetAsync(notificationCacheKey, existingNotification);

            return _mapper.Map<NotificationViewModel>(existingNotification);
        }

        public async Task<List<NotificationViewModel>?> FindAllNotificationsAsync()
        {
            string notificationListCacheKey = "notifications:list";

            var cachedNotifications = await _cacheService.GetAsync<List<Account>>(notificationListCacheKey);
            
            if (cachedNotifications != null)
            {
                return _mapper.Map<List<NotificationViewModel>>(cachedNotifications);
            }

            var existingNotifications = await _notificationRepository.FindAllAsync();
            
            if (existingNotifications == null || existingNotifications.Count == 0)
            {
                return null;
            }

            await _cacheService.SetAsync(notificationListCacheKey, existingNotifications);

            return _mapper.Map<List<NotificationViewModel>>(existingNotifications);
        }

        public async Task<Notification?> CreateNotificationAsync(NotificationInputModel model)
        {
            var notification = model.Adapt<Notification>();
            var validator = _validator.Validate(notification);

            if (!validator.IsValid) return null;

            var createdNotification = await _notificationRepository.Add(notification);

            if (createdNotification != null)
            {
                string accountCacheKey = $"notification:{createdNotification.Id}";

                await _cacheService.SetAsync(accountCacheKey, createdNotification);
            }
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
                
                string accountCacheKey = $"notification:{notificationId}";
                await _cacheService.UpdateCacheIfNeededAsync(accountCacheKey, notification);
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

            string notificationCacheKey = $"notification:{notificationId}";
            await _cacheService.RemoveFromCacheIfNeededAsync<Notification>(notificationCacheKey);

            return true;
        }
    }
}


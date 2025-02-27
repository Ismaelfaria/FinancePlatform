using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Presentation.DTOs.ViewModel
{
    public class NotificationViewModel
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime SentAt { get; private set; }
    }
}

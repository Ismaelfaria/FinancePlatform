using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Presentation.DTOs.InputModel
{
    public class NotificationInputModel
    {
        public Guid AccountId { get; private set; }
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
    }
}

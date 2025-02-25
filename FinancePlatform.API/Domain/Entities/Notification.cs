using FinancePlatform.API.Domain.Enums;

namespace FinancePlatform.API.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime SentAt { get; private set; }

        private Notification() { }

        public Notification(Guid accountId, string message, NotificationType type)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Message = message;
            Type = type;
            SentAt = DateTime.UtcNow;
        }
    }
}

namespace DoDone.Domain.Outbox
{
    public class PendingEmail
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string EmailType { get; set; } = string.Empty;

        public string? Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsSent { get; set; }

        public int RetryCount { get; set; }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Domain.Outbox
{

    public class EmailOutbox
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;   // 👈
        public string UserEmail { get; set; } = string.Empty; // 👈

        public string EmailType { get; set; } = string.Empty;
        public string? Token { get; set; }
        public bool IsSent { get; set; }= false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }


        public static EmailOutbox MarkSent(PendingEmail pending)
        {
            return new EmailOutbox
            {
                Id = pending.Id,
                UserId = pending.UserId,
                EmailType = pending.EmailType,
                Token = pending.Token,
                CreatedAt = pending.CreatedAt,
                RetryCount = pending.RetryCount,
                IsSent = true,
                SentAt = DateTime.UtcNow,
                ErrorMessage = null
            };
        }

        public static EmailOutbox MarkFailed(PendingEmail pending, string errorMessage)
        {
            bool reachedLimit = pending.RetryCount + 1 >= 5;

            return new EmailOutbox
            {
                Id = pending.Id,
                UserId = pending.UserId,
                EmailType = pending.EmailType,
                Token = pending.Token,
                CreatedAt = pending.CreatedAt,
                RetryCount = pending.RetryCount + 1,
                IsSent = reachedLimit,
                SentAt = reachedLimit ? null : null,
                ErrorMessage = errorMessage
            };
        }
    }



}

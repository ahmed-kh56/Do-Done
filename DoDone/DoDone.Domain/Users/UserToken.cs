using ErrorOr;
using DoDone.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Domain.Users
{
    public class UserToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public string Type { get; set; } = "email-confirmation";
        public TokenTypes TokenType { get; set; }
        public string Token { get; set; } =null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;



        private UserToken() { }


        public UserToken(Guid userId, 
            TokenTypes? type, 
            string token,
            DateTime? expiresAt = null)
        {
            UserId = userId;
            Type = type.Name?? TokenTypes.EmailConfirmation.Name;
            Token = token ?? throw new ArgumentNullException(nameof(token), "Token cannot be null.");
            TokenType = type;
        }

        public bool IsExpired( TokenTypes type) => CreatedAt.Add(type.Expiration) < DateTime.UtcNow;



        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow; // Mark the token as used and set the UsedAt timestamp
        }



    }

}

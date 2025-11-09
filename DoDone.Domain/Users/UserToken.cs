namespace DoDone.Domain.Users
{
    public class UserToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public string Type { get; set; } = null!;
        public EmailTemplateType TokenType { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;





        public UserToken(Guid userId,
            EmailTemplateType type,
            string token)
        {
            UserId = userId;
            Token = token ?? throw new ArgumentNullException(nameof(token), "Token cannot be null.");
            TokenType = type;
            Type = type.ToString();
        }


        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow; // Mark the token as used and set the UsedAt timestamp
        }

    }

}
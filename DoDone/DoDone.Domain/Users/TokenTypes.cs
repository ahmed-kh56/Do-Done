using Ardalis.SmartEnum;

namespace DoDone.Domain.Users
{
    public class TokenTypes : SmartEnum<TokenTypes>
    {
        public TimeSpan Expiration { get; }

        private TokenTypes(string name, int value, TimeSpan expiration) : base(name, value)
        {
            Expiration = expiration;
        }

        public static readonly TokenTypes EmailConfirmation =
            new TokenTypes("email-confirmation", 1, TimeSpan.FromHours(24));

        public static readonly TokenTypes PasswordReset =
            new TokenTypes("password-reset", 2, TimeSpan.FromMinutes(15));

        public static readonly TokenTypes RefreshToken =
            new TokenTypes("refresh-token", 3, TimeSpan.FromDays(7));

        public static readonly TokenTypes TwoFactorAuthentication =
            new TokenTypes("2fa", 4, TimeSpan.FromMinutes(5));
    }


}

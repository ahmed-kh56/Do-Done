
namespace DoDone.Domain.Users
{

    public enum EmailTemplateType
    {
        emailConfirmation,
        passwordReset,
        refreshToken,
        TwoFA,
        Welcome,
        FeatureAssigned,
        ProjectAssigned,
        RemoveFromeProject,
        AddStaticRole,
        RemoveStaticRole
    }

}

//public class TokenType : SmartEnum<TokenType>
//{
//    public TimeSpan Expiration { get; }

//    private TokenType(string name, int value, TimeSpan expiration) : base(name, value)
//    {
//        Expiration = expiration;
//    }

//    public static readonly TokenType EmailConfirmation =
//        new TokenType("email-confirmation", 1, TimeSpan.FromHours(24));

//    public static readonly TokenType PasswordReset =
//        new TokenType("password-reset", 2, TimeSpan.FromMinutes(15));

//    public static readonly TokenType RefreshToken =
//        new TokenType("refresh-token", 3, TimeSpan.FromDays(7));

//    public static readonly TokenType TwoFactorAuthentication =
//        new TokenType("2fa", 4, TimeSpan.FromMinutes(5));
//    public bool IsExpired(DateTime createdAt)
//    {
//        return DateTime.UtcNow > createdAt.Add(Expiration);
//    }
//}
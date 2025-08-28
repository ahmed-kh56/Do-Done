
namespace DoDone.Authentication;

public record LoginWithRefreshTokenRequest(
    string RefreshToken,
    Guid UserId);

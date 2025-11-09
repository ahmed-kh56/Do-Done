
namespace DoDone.Application.Common.Dtos.Authentication;

public record LoginWithRefreshTokenRequest(
    string RefreshToken,
    Guid UserId);

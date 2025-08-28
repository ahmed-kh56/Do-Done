using DoDone.Domain.Users;

namespace DoDone.Application.Queries.Authentication;

public record AuthenticationResult(
    User User,
    string Token);
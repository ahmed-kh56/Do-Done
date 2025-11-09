using DoDone.Domain.Users;

namespace DoDone.Application.Common.Dtos.UserDtos;

public record AuthenticationResult(
    User User,
    string Token);
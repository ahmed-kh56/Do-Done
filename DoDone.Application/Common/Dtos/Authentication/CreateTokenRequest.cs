namespace DoDone.Application.Common.Dtos.Authentication;
public record CreateTokenRequest(
    string Email,
    string Type);


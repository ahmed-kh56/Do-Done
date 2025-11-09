namespace DoDone.Application.Common.Dtos.Authentication;

public record ConfirmEmailRequest(
    string Email,
    string Token,
    Guid UserId);



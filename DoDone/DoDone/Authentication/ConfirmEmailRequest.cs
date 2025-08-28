namespace DoDone.Authentication;

public record ConfirmEmailRequest(
    string Email,
    string Token,
    Guid UserId);



namespace DoDone.Application.Common.Dtos.Authentication;

public record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword);



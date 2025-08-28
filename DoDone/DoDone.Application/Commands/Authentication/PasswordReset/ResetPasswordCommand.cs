using ErrorOr;
using MediatR;
using DoDone.Application.Queries.Authentication;


namespace DoDone.Application.Commands.Authentication.PasswordReset;

public partial record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword
) : IRequest<ErrorOr<AuthenticationResult>>;



using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.Authentication.Login;
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
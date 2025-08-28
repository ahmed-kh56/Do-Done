using ErrorOr;

using MediatR;
using DoDone.Application.Authentication.Common;

namespace DoDone.Application.Queries.Authentication.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
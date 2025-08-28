using ErrorOr;

using MediatR;
using DoDone.Application.Authentication.Common;
using DoDone.Application.Queries.Authentication;

namespace DoDone.Application.Commands.Authentication.Register;

public record RegisterCommand(
    string FullName,
    string ShowName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
using DoDone.Application.Common.Dtos.UserDtos;
using ErrorOr;

using MediatR;

namespace DoDone.Application.Commands.Authentication.Register;

public record RegisterCommand(
    string FullName,
    string ShowName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
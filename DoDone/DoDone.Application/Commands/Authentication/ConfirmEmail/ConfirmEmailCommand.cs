using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Authentication.ConfirmEmail;

public record ConfirmEmailCommand (string Email,string Token,Guid UserId) : IRequest<ErrorOr<Success>>;


using DoDone.Application.Common.Authorization;
using DoDone.Domain.Projects;
using DoDone.Domain.Users;
using ErrorOr;
using MediatR;
using System;


namespace DoDone.Application.Commands.Authentication.CreateToken;
public record CreateTokenCommand(string Email, string Type) : IRequest<ErrorOr<Success>>;



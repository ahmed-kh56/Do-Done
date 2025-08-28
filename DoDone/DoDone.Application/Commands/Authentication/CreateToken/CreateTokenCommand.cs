using ErrorOr;
using DoDone.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Commands.Authentication.CreateToken;
    public record CreateTokenCommand(string Email, string Type) : IRequest<ErrorOr<Success>>;



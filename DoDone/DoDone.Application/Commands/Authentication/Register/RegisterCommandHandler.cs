using ErrorOr;
using MediatR;
using DoDone.Domain.Users;
using DoDone.Domain.Common;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Queries.Authentication;

namespace DoDone.Application.Commands.Authentication.Register;

public class RegisterCommandHandler(
    ITokenGenerator _tokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistsByEmailAsync(command.Email))
        {
            return Error.Conflict(description: "User already exists");
        }

        var hashPasswordResult = _passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = new User(
            command.FullName,
            command.ShowName,
            command.Email,
            hashPasswordResult.Value);


        await _usersRepository.AddUserAsync(user);
        

        var token = _tokenGenerator.GenerateJwtToken(user);

        return new AuthenticationResult(user, token);
    }
}
using ErrorOr;

using MediatR;
using DoDone.Domain.Common;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Errors;


namespace DoDone.Application.Queries.Authentication.Login;

public class LoginQueryHandler(
    ITokenGenerator _tokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository,
    IProjectUserRolesRepository _rolesRepository)
        : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(query.Email);
        
        if (user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher))
        {
            return AuthenticationErrors.InvalidCredentials;
        }
        var userRoles = await _rolesRepository.GetActiveUserRolesAsync(user.Id);

        return new AuthenticationResult(user, _tokenGenerator.GenerateJwtToken(user,userRoles.ToList()));
    }

}

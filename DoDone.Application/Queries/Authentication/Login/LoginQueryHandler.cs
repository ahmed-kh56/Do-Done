using ErrorOr;
using MediatR;
using DoDone.Domain.Common;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Errors;
using DoDone.Domain.Users;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Dtos.Roles;


namespace DoDone.Application.Queries.Authentication.Login;

public class LoginQueryHandler(
    ITokenGenerator _tokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository,
    IRolesRepository _rolesRepository)
        : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {

        var user = await _usersRepository.GetByEmailAsync(query.Email);

        if (user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher))
        {
            return AuthenticationErrors.InvalidCredentials;
        }
        var userRoles = await _rolesRepository.GetUserRolesAsync(user.Id);


        return new AuthenticationResult(user, _tokenGenerator.GenerateJwtToken(user,userRoles.ToList()));
    }

}

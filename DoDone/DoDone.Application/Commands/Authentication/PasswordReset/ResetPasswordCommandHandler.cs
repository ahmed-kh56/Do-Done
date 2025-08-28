using ErrorOr;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Authentication.Common;
using DoDone.Domain.Common;
using DoDone.Domain.Users;
using MediatR;
using DoDone.Application.Queries.Authentication;


namespace DoDone.Application.Commands.Authentication.PasswordReset
{
    public class ResetPasswordCommandHandler (
        IUsersRepository _usersRepository,
        ITokenGenerator _tokenGenerator,
        IUserTokensRepository _tokenRepsitory,
        IPasswordHasher _pawwwordHasher,
        IProjectUserRolesRepository _rolesRepository
        ) : IRequestHandler<ResetPasswordCommand, ErrorOr<AuthenticationResult>>
    {
        public async Task<ErrorOr<AuthenticationResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ تجيب اليوزر
            var user = await _usersRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Error.Custom(code: "NotFound", description: "No account with this email.", type: 4);
            }


            var statusCode = await _tokenRepsitory.UseTokenAsync(user.Id, request.Token, TokenTypes.PasswordReset.Name);

            if (statusCode == 404)
            {
                return Error.Custom(code: "InvalidToken", description: "The provided token is invalid or already used.", type: 4);
            }

            if (statusCode == 0)
            {
                return Error.Custom(code: "TokenExpired", description: "The token has expired.", type: 4);
            }

            var roles = await _rolesRepository.GetActiveUserRolesAsync(user.Id);
            var authToken = _tokenGenerator.GenerateJwtToken(user, roles.ToList());

            var passwordHashResult = user.UpdatePassword(request.NewPassword, _pawwwordHasher);
            if (passwordHashResult.IsError)
            {
                return passwordHashResult.Errors;
            }

            _usersRepository.Update(user);

            return new AuthenticationResult(user, authToken);
        }

    }
}

using ErrorOr;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Common;
using DoDone.Domain.Users;
using MediatR;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Dtos.Roles;


namespace DoDone.Application.Commands.Authentication.PasswordReset
{
    public class ResetPasswordCommandHandler (
        IUsersRepository _usersRepository,
        ITokenGenerator _tokenGenerator,
        IUserTokensRepository _tokenRepsitory,
        IPasswordHasher _pawwwordHasher,
        IRolesRepository _rolesRepository,
        IUnitOfWork _unitOfWork)
        : IRequestHandler<ResetPasswordCommand, ErrorOr<AuthenticationResult>>
    {
        public async Task<ErrorOr<AuthenticationResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            User user = await _usersRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Error.Custom(code: "NotFound", description: "No account with this email.", type: 4);
            }


            int statusCode = 200;
            ErrorOr<Success> passwordHashResult = new Success();
            await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
            {

                statusCode = await _tokenRepsitory.UseTokenAsync(user.Id, request.Token, EmailTemplateType.passwordReset.ToString());
                if (statusCode == 200)
                {
                    var passwordHashResult = user.UpdatePassword(request.NewPassword, _pawwwordHasher);
                    if (!passwordHashResult.IsError)
                    {
                        await _usersRepository.Update(user);
                    }
                }

            });
            if(passwordHashResult.IsError)
            {
                return passwordHashResult.Errors;
            }



            var roles = await _rolesRepository.GetUserRolesAsync(user.Id);

            var authToken = _tokenGenerator.GenerateJwtToken(user, roles.ToList());



            return new AuthenticationResult(user, authToken);
        }

    }
}

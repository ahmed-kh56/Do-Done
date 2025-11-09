using ErrorOr;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Users;
using MediatR;


namespace DoDone.Application.Commands.Authentication.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(
        IUsersRepository _usersRepository,
        IUserTokensRepository _tokenRepsitory)
        : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>


    {
        public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var statusCode = await _tokenRepsitory.UseTokenAsync(request.UserId, request.Token, EmailTemplateType.emailConfirmation.ToString());

            if (statusCode == 404)
            {
                return Error.Custom(code: "NotFound", description: "No, Something Went Wrong.", type: 3);
            }

            if (statusCode == 0)
            {
                return Error.Custom(code: "Expired", description: "The token has expired.", type: 3);
            }

            var user = await _usersRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Error.Custom(code: "NotFound", description: "User not found.", type: 3);
            }

            user.ConfirmEmail();

            _usersRepository.Update(user);

            return new Success();
        }

    }
}

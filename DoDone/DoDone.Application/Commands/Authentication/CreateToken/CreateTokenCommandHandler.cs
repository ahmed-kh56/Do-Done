using ErrorOr;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Helpers;
using DoDone.Domain.Users;
using MediatR;


namespace DoDone.Application.Commands.Authentication.CreateToken
{
    public class CreateTokenCommandHandler(
        IUsersRepository _usersRepository,
        ITokenGenerator _tokenGenerator,
        IUserTokensRepository _tokenRepsitory,
        IEmailService _emailService)
        : IRequestHandler<CreateTokenCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {


            var user = await _usersRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Error.Custom(code: "NotFound", description: "No, Account With this Email.", type: 4);
            }

            TokenTypes tokenType;

            try
            {
                tokenType = TokenTypes.FromName(request.Type, ignoreCase: true);
            }
            catch (Exception)
            {
                return Error.Custom(code: "InvalidTokenType", description: "Invalid Token Type.", type: 4);
            }
            

            var token =  _tokenGenerator.GenerateUserTokens(user,tokenType);

            
            await _tokenRepsitory.AddAsync(token);

            await _emailService.SendEmailAsync(
                request.Email,
                $"Your {request.Type} Token From DoDone",
                EmailBodyTemplates.GenerateTemplate(user.ShowName, token.Token, TokenTypes.FromName(token.Type))
            );

            return new Success();

        }
    }
}

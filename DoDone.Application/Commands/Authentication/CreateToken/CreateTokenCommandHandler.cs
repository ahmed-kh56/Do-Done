using ErrorOr;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Users;
using MediatR;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Outbox;


namespace DoDone.Application.Commands.Authentication.CreateToken
{
    public class CreateTokenCommandHandler(
        IUsersRepository _usersRepository,
        ITokenGenerator _tokenGenerator,
        IUserTokensRepository _tokenRepsitory,
        IEmailOutboxRepository _outboxRepository,
        IUnitOfWork _unitOfWork)
        : IRequestHandler<CreateTokenCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {


            var user = await _usersRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Error.Custom(code: "NotFound", description: "No, Account With this Email.", type: 4);
            }

            EmailTemplateType tokenType;

            try
            {
                tokenType = (EmailTemplateType)Enum.Parse(typeof(EmailTemplateType),request.Type);
            }
            catch (Exception)
            {
                return Error.Custom(code: "InvalidTokenType", description: "Invalid Token Type.", type: 4);
            }

            var token =  _tokenGenerator.GenerateUserTokens(user,tokenType);
            var EmailToBesent = new EmailOutbox
            {
                Id= Guid.NewGuid(),
                UserEmail = user.Email,
                UserId = user.Id,
                UserName = user.ShowName,
                EmailType = tokenType.ToString(),
                Token = token.Token
            };
            await _unitOfWork.ExecuteInTransactionAsync(async (conn,tran) =>
            {
                await _tokenRepsitory.AddAsync(token,conn,tran);
                await _outboxRepository.AddAsync(EmailToBesent, conn,tran);
            });



            return new Success();

        }
    }
}

using ErrorOr;
using MediatR;
using DoDone.Domain.Users;
using DoDone.Domain.Common;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Domain.Outbox;

namespace DoDone.Application.Commands.Authentication.Register;

public class RegisterCommandHandler(
    ITokenGenerator _tokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository,
    IUnitOfWork _unitOfWork,
    IEmailOutboxRepository _emailOutboxRepository)
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

        var emailOutbox = new EmailOutbox
        {
            Id = Guid.NewGuid(),
            UserEmail = user.Email,
            UserId = user.Id,
            UserName = user.ShowName,
            EmailType = EmailTemplateType.Welcome.ToString(),
            Token = null
        };
        await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
        {
            await _usersRepository.AddUserAsync(user, conn, tran);
            await _emailOutboxRepository.AddAsync(emailOutbox, conn, tran);
        });


        var token = _tokenGenerator.GenerateJwtToken(user);

        return new AuthenticationResult(user, token);
    }
}
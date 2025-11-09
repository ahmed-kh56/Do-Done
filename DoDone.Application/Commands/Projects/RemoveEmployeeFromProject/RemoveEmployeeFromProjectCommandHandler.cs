using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Outbox;
using DoDone.Domain.Users;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.RemoveEmployeeFromProject;

public class RemoveEmployeeFromProjectCommandHandler (
    IPURRepository _pURRepository,
    IFeatureRepository _featureRepository,
    IUnitOfWork _unitOfWork,
    IUsersRepository _usersRepository,
    IProjectRepository _projectRepository,
    IEmailOutboxRepository _emailOutboxRepository): IRequestHandler<RemoveEmployeeFromProjectCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(RemoveEmployeeFromProjectCommand request, CancellationToken cancellationToken)
    {

        var pur = (await _pURRepository.GetByIdAsync(request.purId)).FirstOrDefault();
        if (pur is null)
            return Error.Conflict( description:"The user is not assigned to the project.");

        var project = await _projectRepository.GetByIdAsync(pur.ProjectId);
        if (project is null)
            return Error.NotFound(description: "Project not found.");

        var user = await _usersRepository.GetByIdAsync(pur.UserId);
        var email = new EmailOutbox
        {
            Id = Guid.NewGuid(),
            UserId = pur.UserId,
            EmailType = EmailTemplateType.RemoveFromeProject.ToString(),
            UserEmail = user.Email,
            UserName = user.FullName,
            Token = project.Name,
        };

        await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
        {
            await _pURRepository.EndAsync(
                id: pur.Id,
                dbConnection: conn,
                dbTransaction: tran);
            await _featureRepository.UnassaignAllAsync(
                purId: pur.Id,
                dbConnection: conn,
                dbTransaction: tran);
            await _emailOutboxRepository.AddAsync(email, conn, tran);
        });
        return Result.Success;
    }
}
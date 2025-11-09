using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Features;
using DoDone.Domain.Projects;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.EndProject
{

    public class EndProjectCommandHandler(
        IProjectRepository _projectRepository,
        IPURRepository _purRepository,
        ITaskItemRepository _tasksRepository,
        IUnitOfWork _unitOfWork) : IRequestHandler<EndProjectCommand, ErrorOr<Success>>
    {

        public async Task<ErrorOr<Success>> Handle(EndProjectCommand request, CancellationToken cancellationToken)
        {
            Project project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                return Error.NotFound("Project.NotFound", "Project not found.");
            }


            if (project is null)
            {
                return Error.NotFound("Project.NotFound", "Project not found.");
            }
            if (project.IsEnded)
            {
                return Error.Validation("Project.AlreadyEnded", "Project is already ended.");
            }


            project.End();
            await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
            {
                await _projectRepository.UpdateAsync(project,conn,tran);
                await _purRepository.MarkAllAsEndedAsync(request.ProjectId, conn, tran);
                await _tasksRepository.MarkAllAsAsync(
                    status: TaskItemStatus.ProjectEnded,
                    ProjectId: request.ProjectId,
                    dbConnection: conn,
                    dbTransaction: tran);

            });

            return Result.Success;
        }

    }




}
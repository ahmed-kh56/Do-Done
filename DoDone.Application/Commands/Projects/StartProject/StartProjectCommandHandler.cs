using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Projects;
using DoDone.Domain.Roles;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.StartProject;

public class StartProjectCommandHandler(
    IProjectRepository _projectRepository,
    IPURRepository _pURRepository,
    IRolesRepository _rolesRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<StartProjectCommand, ErrorOr<Project>>
{
    public async Task<ErrorOr<Project>> Handle(StartProjectCommand request, CancellationToken cancellationToken)
    {
        byte employeeRoleId = await _rolesRepository.GetRoleIdByNameAsync(AuthNames.Employee);

        if (employeeRoleId == 0)
        {
            return Error.NotFound("Role.NotFound", "Employee role not found.");
        }
        var projectToBeAdded = new Project(
            request.Name,
            request.IsStarted,
            request.StartDate??DateTime.UtcNow);

        var purToBeAdded = new ProjectUserRole(
            userId: AuthNames.DefaultEmployeeId,
            projectId: projectToBeAdded.Id,
            roleId: employeeRoleId,
            id: projectToBeAdded.Id);

        await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
        {
            await _projectRepository.AddProjectAsync(projectToBeAdded, conn, tran);
            await _pURRepository.AssignUserToProjectAsync(purToBeAdded, conn, tran);
        });

        return projectToBeAdded;
    }
}
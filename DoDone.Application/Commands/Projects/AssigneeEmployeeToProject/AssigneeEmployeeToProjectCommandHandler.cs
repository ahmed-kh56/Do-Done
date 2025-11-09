using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Outbox;
using DoDone.Domain.Projects;
using DoDone.Domain.Users;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Commands.Projects.AssigneeEmployeeToProject
{
    public class AssigneeEmployeeToProjectCommandHandler (
        IPURRepository _pURRepository,
        IProjectRepository _projectRepository,
        IRolesRepository _rolesRepository,
        IUsersRepository _userRepository,
        IUnitOfWork _unitOfWork,
        IEmailOutboxRepository _emailOutboxRepository): IRequestHandler<AssigneeEmployeeToProjectCommand, ErrorOr<Guid>>
    {
        public async Task<ErrorOr<Guid>> Handle(AssigneeEmployeeToProjectCommand request, CancellationToken cancellationToken)
        {

            if (await _pURRepository.IsUserInProjectAsync(request.UserId, request.ProjectId))
                return Error.Conflict(description: "User already assigned to this project or project not exists");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Error.NotFound(description: "User not exists");

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
                return Error.NotFound(description: "Project not exists");



            byte roleId = await _rolesRepository.GetRoleIdByNameAsync(request.RoleName);
            if (roleId == 0)
                return Error.NotFound(description: "Role not found");


            var pur = new ProjectUserRole(
                userId: request.UserId,
                projectId: request.ProjectId,
                roleId:roleId);

            var email = new EmailOutbox
            {
                Id = Guid.NewGuid(),
                UserEmail = user.Email,
                UserId = user.Id,
                UserName = user.ShowName,
                Token = $"{project.Name}|{request.RoleName}",
                EmailType = EmailTemplateType.ProjectAssigned.ToString()

            };


            await _unitOfWork.ExecuteInTransactionAsync(async (conn, tran) =>
            {
                await _pURRepository.AssignUserToProjectAsync(pur, conn, tran);
                await _emailOutboxRepository.AddAsync(email, conn, tran);
            });
            return pur.Id;

        }
    }

}

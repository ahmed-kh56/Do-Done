using DoDone.Application.Commands.Projects.AssigneeEmployeeToProject;

namespace DoDone.Application.Common.Dtos.ProjectDtos
{
    public record AssigneeEmployeeToProjectRequest
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string RoleName { get; set; }
        public AssigneeEmployeeToProjectCommand ToCommand()
        {
            return new AssigneeEmployeeToProjectCommand(UserId, ProjectId, RoleName);
        }

    }

}

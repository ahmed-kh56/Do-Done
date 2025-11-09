using DoDone.Application.Common.Authorization;
using FluentValidation;

namespace DoDone.Application.Commands.Projects.AssigneeEmployeeToProject
{
    public class AssigneeEmployeeToProjectValidator :AbstractValidator<AssigneeEmployeeToProjectCommand>
    {
        IReadOnlyCollection<string> validRoles = new List<string> { AuthNames.ScrumMaster,AuthNames.Employee }.AsReadOnly();
        public AssigneeEmployeeToProjectValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty).WithMessage("UserId is required");
            RuleFor(x => x.ProjectId).NotEmpty().NotEqual(Guid.Empty).WithMessage("ProjectId is required");
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("RoleName is required");
            RuleFor(x => x.RoleName).Must(role => validRoles.Contains(role))
                .WithMessage($"RoleName must be one of the following: {string.Join(", ", validRoles)}");
        }
    }


}

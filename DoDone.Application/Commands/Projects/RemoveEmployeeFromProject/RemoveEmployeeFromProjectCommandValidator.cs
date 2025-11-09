using FluentValidation;

namespace DoDone.Application.Commands.Projects.RemoveEmployeeFromProject;

public class RemoveEmployeeFromProjectCommandValidator:AbstractValidator<RemoveEmployeeFromProjectCommand>
{
    public RemoveEmployeeFromProjectCommandValidator()
    {
        RuleFor(x => x.purId).NotEqual(Guid.Empty)
            .WithMessage("Project User Role Id cannot be an empty GUID.");
    }

}
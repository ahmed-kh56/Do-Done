using DoDone.Application.Commands.Projects.StartProject;
using FluentValidation;

namespace DoDone.Application.Commands.Projects.StartProject;

public class StartProjectCommandVAlidator:AbstractValidator<StartProjectCommand>
{
    public StartProjectCommandVAlidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().NotNull()
            .MaximumLength(256)
            .MinimumLength(3);
        RuleFor(x => x.IsStarted)
            .NotEmpty();

    }

}
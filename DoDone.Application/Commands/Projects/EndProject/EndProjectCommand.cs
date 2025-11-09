using DoDone.Application.Common.Authorization;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace DoDone.Application.Commands.Projects.EndProject;

[Authorize(Role = "Manager")]
public record EndProjectCommand(Guid ProjectId):IRequest<ErrorOr<Success>>;



public class EndProjectCommandValidator : AbstractValidator<EndProjectCommand>
{
    public EndProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId is required.");
    }
}

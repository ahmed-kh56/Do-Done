using DoDone.Application.Common.Authorization;
using DoDone.Domain.Projects;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace DoDone.Application.Queries.Projects.GetProjectById
{
    [AuthorizationLogic(mode: AuthorizationMode.Or)]
    [Authorize(Role ="Manager")]
    public record GetProjectByIdQuery(Guid ProjectId) : IRequest<ErrorOr<Project>>, IProjectAuthorizationRequest
    {
        Guid IProjectAuthorizationRequest.ProjectId => ProjectId;

        Guid? IProjectAuthorizationRequest.FeatureId => null;

        IEnumerable<DynamicRoleRequirement> IProjectAuthorizationRequest.RequiredDynamicRoles =>new[] { AuthNames.ScrumMasterRequirement };
    }
    public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
    {
        public GetProjectByIdQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}

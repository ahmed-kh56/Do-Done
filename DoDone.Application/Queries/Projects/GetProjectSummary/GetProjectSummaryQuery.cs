using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Dtos.ProjectDtos;
using ErrorOr;
using MediatR;


namespace DoDone.Application.Queries.Projects.GetProjectSummary
{
    [AuthorizationLogic(AuthorizationMode.Or)]
    [Authorize(Role = "Manager")]
    public record GetProjectSummaryQuery(Guid ProjectId)
        : IRequest<ErrorOr<ProjectSummaryDto>>, IProjectAuthorizationRequest
    {
        Guid? IProjectAuthorizationRequest.FeatureId => null;
        Guid IProjectAuthorizationRequest.ProjectId => ProjectId;

        IEnumerable<DynamicRoleRequirement> IProjectAuthorizationRequest. RequiredDynamicRoles => new[] {
            AuthNames.ScrumMasterRequirement,
            AuthNames.ProjectEmployeeRequirement
        };

    }

}

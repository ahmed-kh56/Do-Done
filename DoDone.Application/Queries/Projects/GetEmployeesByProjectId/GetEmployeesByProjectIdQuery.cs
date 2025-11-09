using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Dtos.UserDtos;
using ErrorOr;
using MediatR;


namespace DoDone.Application.Queries.Projects.GetEmployeesByProjectId
{
    [AuthorizationLogic(mode: AuthorizationMode.Or)]
    [Authorize(Role = "Manager")]
    public record GetEmployeesByProjectIdQuery(Guid ProjectId)
        : IRequest<ErrorOr<IEnumerable<ProjectEmployeeProfileResult>>>, IProjectAuthorizationRequest
    {
        Guid IProjectAuthorizationRequest.ProjectId => ProjectId;
        Guid? IProjectAuthorizationRequest.FeatureId => null;
        IEnumerable<DynamicRoleRequirement> IProjectAuthorizationRequest.RequiredDynamicRoles => new[] 
        {
            AuthNames.ScrumMasterRequirement
        };
    }
}

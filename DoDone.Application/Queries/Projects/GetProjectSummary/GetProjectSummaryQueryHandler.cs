using DoDone.Application.Common.Dtos;
using DoDone.Application.Common.Dtos.ProjectDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.Projects.GetProjectSummary
{
    public class GetProjectSummaryQueryHandler(IProjectRepository _projectRepository) : IRequestHandler<GetProjectSummaryQuery, ErrorOr<ProjectSummaryDto>>
    {
        public async Task<ErrorOr<ProjectSummaryDto>> Handle(GetProjectSummaryQuery request, CancellationToken cancellationToken)
        {

            var projectSummary = await _projectRepository.GetProjectManagerialDitals(request.ProjectId);
            if (projectSummary == null)
            {
                return Error.NotFound(code: "ProjectNotFound", description: $"Project with Id {request.ProjectId} not found.");
            }
            return projectSummary;

        }
    }
        
}

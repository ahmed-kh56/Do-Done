using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Projects;
using ErrorOr;
using MediatR;

namespace DoDone.Application.Queries.Projects.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ErrorOr<Project>>
    {
        private readonly IProjectRepository _projectRepository;
        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<ErrorOr<Project>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project is null)
            {
                return Error.NotFound("Project.NotFound", "Project not found.");
            }
            return project;
        }
    }
}

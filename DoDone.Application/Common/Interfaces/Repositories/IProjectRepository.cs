using DoDone.Application.Common.Dtos.ProjectDtos;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Domain.Projects;
using System.Data;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectSummaryDto?> GetProjectManagerialDitals(Guid projectId);
        Task<bool> IsProjectExistsAsync(Guid projectId);
        Task AddProjectAsync(Project projectToBeAdded, IDbConnection dbConnection = null, IDbTransaction dbTransaction = null);
        Task<ProjectSummaryDto?> GetProjectSummaryByIdAsync(Guid projectId);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task UpdateAsync(Project projectToBeUpdated, IDbConnection dbConnection = null, IDbTransaction dbTransaction = null);
    }
}

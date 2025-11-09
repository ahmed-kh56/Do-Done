using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Domain.Projects;
using System.Data;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IPURRepository
    {
        Task<bool> IsUserInProjectAsync(
            Guid? userId = null,
            Guid? projectId = null,
            Guid? projectUserRoleId = null,
            bool activeOnly = true);
        Task<IEnumerable<ProjectEmployeeProfileResult>> GetEmployeesByProjectId(Guid projectId, bool fullHistory = false);
        Task AssignUserToProjectAsync(
            ProjectUserRole pur,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null);
        Task EndAsync(
            Guid? projectId = null,
            Guid? userId = null,
            Guid? id = null,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null);
        Task UpdateRoleAsync(
            byte roleId,
            Guid? projectId = null,
            Guid? userId = null,
            Guid? id = null,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null);
        Task MarkAllAsEndedAsync(
            Guid projectId,
            IDbConnection dbConnection,
            IDbTransaction dbTransaction);
        Task<IEnumerable<ProjectUserRole>> GetByIdAsync(
            Guid? purId = null,
            Guid? projectId = null,
            Guid? userId = null,
            bool onlyActive = true);
    }

}

using Dapper;
using DoDone.Application.Common.Dtos.Features;
using DoDone.Application.Common.Dtos.ProjectDtos;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Projects;
using DoDone.Infrastructure.Persistence.DbSettings;
using Microsoft.Extensions.Options;
using System.Data;


namespace DoDone.Infrastructure.Persistence.Repositories.Projects
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly IDbSettings _dbSettings;

        public ProjectRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }




        public async Task<ProjectSummaryDto?> GetProjectManagerialDitals(Guid projectId)
        {
            var command = "[Reading.ProjectData].GetProjectSummaryById";
            var parameters = new { ProjectId = projectId };

            using var conn = _dbSettings.CreateConnection();

            var result = await conn.QueryFirstOrDefaultAsync<ProjectSummaryDto>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }


        public async Task<ProjectSummaryDto?> GetProjectSummaryByIdAsync(Guid projectId)
        {
            var command = "[Reading.ProjectData].GetProjectDetailsById";
            var parameters = new { ProjectId = projectId };

            using var conn = _dbSettings.CreateConnection();
            using var multi = await conn.QueryMultipleAsync(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure);

            var project = await multi.ReadFirstOrDefaultAsync<ProjectSummaryDto>();
            if (project == null) return null;

            var employees = (await multi.ReadAsync<ProjectEmployeeProfileResult>()).ToList();

            var features = (await multi.ReadAsync<FeatureSummaryDto>()).ToList();
            if (features.Any() && !employees.Any())
            {
                project.UnAssignedFeatures = features.Where(f=>f.AssinedPURId==Guid.Empty);
                return project;
            }

            var featuresByEmployee = features
                .GroupBy(f => f.AssinedPURId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var emp in employees)
            {
                emp.features = featuresByEmployee.TryGetValue(emp.Id, out var empFeatures)
                    ? empFeatures
                    : new List<FeatureSummaryDto>();
            }

            project.UnAssignedFeatures = featuresByEmployee.TryGetValue(projectId, out var unassigned)
                ? unassigned
                : new List<FeatureSummaryDto>();


            project.Employees = employees;


            return project;
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            var command = "[Reading.ProjectData].[GetProjectById]";
            var parameters = new { ProjectId = projectId };

            using var conn = _dbSettings.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<Project>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure);

        }

        public async Task AddProjectAsync(
            Project projectToBeAdded,
            IDbConnection dbConnection = null,
            IDbTransaction dbTransaction = null
            )
        {
            var command = "[Writing.ProjectData].[AddProject]";
            var parameters = new
            {
                projectToBeAdded.Id,
                projectToBeAdded.Name,
                projectToBeAdded.CreatedAt,
                projectToBeAdded.IsStarted,
                projectToBeAdded.StartDate,
                projectToBeAdded.IsCompleted,
                projectToBeAdded.EndDate
            };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();


            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (dbTransaction != null && dbTransaction.Connection != connection)
                throw new InvalidOperationException("Transaction must be associated with the same connection.");

            await connection.ExecuteAsync(
                sql: command,
                param: parameters,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            if (!externalConnection)
                connection.Dispose();

        }



        public async Task UpdateAsync(
            Project projectToBeUpdated,
            IDbConnection dbConnection = null,
            IDbTransaction dbTransaction = null
            )
        {
            var command = "[Writing.ProjectData].[UpdateProject]";
            var parameters = new
            {
                projectToBeUpdated.Id,
                projectToBeUpdated.Name,
                projectToBeUpdated.IsStarted,
                projectToBeUpdated.StartDate,
                projectToBeUpdated.IsCompleted,
                projectToBeUpdated.EndDate
            };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();


            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (dbTransaction != null && dbTransaction.Connection != connection)
                throw new InvalidOperationException("Transaction must be associated with the same connection.");

            await connection.ExecuteAsync(
                sql: command,
                param: parameters,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            if (!externalConnection)
                connection.Dispose();
        }

        public Task<bool> IsProjectExistsAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }


}

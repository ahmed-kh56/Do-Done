using Dapper;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Projects;
using DoDone.Infrastructure.Persistence.DbSettings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Persistence.Repositories.Projects
{
    internal class PURRepository: IPURRepository
    {

        private readonly IDbSettings _dbSettings;

        public PURRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        public async Task<bool> IsUserInProjectAsync(
            Guid? userId = null,
            Guid? projectId = null,
            Guid? projectUserRoleId = null,
            bool activeOnly = true)
        {
            var command = "[Reading.ProjectData].[IsUserInProject]";
            var parameters = new
            {
                UserId = userId,
                ProjectId = projectId,
                ProjectUserRoleId = projectUserRoleId,
                ActiveOnly = activeOnly
            };

            using var connection = _dbSettings.CreateConnection();

            return await connection.ExecuteScalarAsync<bool>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }



        public async Task AssignUserToProjectAsync(
            ProjectUserRole pur,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null)
        {
            var command = @"[Writing.ProjectData].[AssignUserToProject]";
            var parameters = new
            {
                pur.Id,
                pur.UserId,
                pur.ProjectId,
                pur.RoleId,
                pur.StartedAt
            };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (dbTransaction != null && dbTransaction.Connection != connection)
                throw new InvalidOperationException("Transaction must be associated with the same connection.");

            await connection.ExecuteAsync(
                command,
                parameters,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            if (!externalConnection)
                connection.Dispose();
        }


        public async Task UpdateRoleAsync(
            byte roleId,
            Guid? projectId = null,
            Guid? userId = null,
            Guid? id = null,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null)
        {
            var command = @"[Writing.ProjectData].[UpdateUserRoleInProject]";
            var parameters = new
            {
                PurId = id,
                ProjectId = projectId,
                UserId = userId,
                RoleId = roleId
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



        public async Task<IEnumerable<ProjectEmployeeProfileResult>> GetEmployeesByProjectId(
            Guid projectId,
            bool fullHistory = false)
        {
            var command = "[Reading.ProjectData].GetProjectEmployeesByProjectId";
            var parameters = new { ProjectId = projectId, Historical = fullHistory };

            using var conn = _dbSettings.CreateConnection();
            var result = await conn.QueryAsync<ProjectEmployeeProfileResult>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }



        public async Task EndAsync(
            Guid? projectId = null,
            Guid? userId = null,
            Guid? id = null,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null)

        {
            var command = @"[Writing.ProjectData].[MarkProjectEmployeesAsEnded]";
            var parameters = new { ProjectId = projectId, UserId = userId, PurId = id, LeftAt = DateTime.Now };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();


            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (dbTransaction != null && dbTransaction.Connection != connection)
                throw new InvalidOperationException("Transaction must be associated with the same connection.");


            await connection.ExecuteAsync(
                command,
                parameters,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure
            );
            if (!externalConnection)
                connection.Dispose();
        }




        public async Task MarkAllAsEndedAsync(
            Guid projectId,
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            var command = @"[Writing.ProjectData].[MarkProjectEmployeesAsEnded]";
            var parameters = new { ProjectId = projectId, UserId = (Guid?)null, PurId = (Guid?)null, LeftAt = DateTime.Now};

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();


            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (dbTransaction != null && dbTransaction.Connection != connection)
                throw new InvalidOperationException("Transaction must be associated with the same connection.");


            await connection.ExecuteAsync(
                command,
                parameters,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure
            );
            if (!externalConnection)
                connection.Dispose();
        }

        public async Task<IEnumerable<ProjectUserRole>> GetByIdAsync(
            Guid? purId = null,
            Guid? projectId = null,
            Guid? userId = null,
            bool onlyActive = true)
        {
            var command = "[Reading.ProjectData].[GetProjectUserRoles]";
            var parameters = new
            {
                PurId = purId,
                ProjectId = projectId,
                UserId = userId,
                OnlyActive = onlyActive
            };

            using var connection = _dbSettings.CreateConnection();

            return await connection.QueryAsync<ProjectUserRole>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}

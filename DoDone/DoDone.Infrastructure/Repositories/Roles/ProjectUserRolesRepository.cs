using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Roles;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DoDone.Application.Queries.Authentication;
using Microsoft.Extensions.Options;
using HealLink.Infrastructure;


namespace DoDone.Infrastructure.Repositories.Roles
{
    internal class ProjectUserRolesRepository : IProjectUserRolesRepository
    {
        private readonly string _connectionString;

        public ProjectUserRolesRepository(IOptions<DbSettings> options)
        {
            _connectionString = options.Value.ConnectionSting;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<IEnumerable<UserProjectRoleDto>?> GetActiveUserRolesAsync(Guid userId, bool onlyActive =false)
        {
            var Command = $"[Reading].[GetUserProjects]";
            using (var connection = CreateConnection())
                return await connection.QueryAsync<UserProjectRoleDto>
                    (Command, new { UserId = userId , OnlyActive = onlyActive }, commandType: CommandType.StoredProcedure);
        }
    }
}

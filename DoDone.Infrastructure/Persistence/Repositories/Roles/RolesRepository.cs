using Dapper;
using DoDone.Application.Common.Dtos.Roles;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Roles;
using DoDone.Infrastructure.Persistence.DbSettings;
using Microsoft.Extensions.Options;
using System.Data;

namespace DoDone.Infrastructure.Persistence.Repositories.Roles
{
    public class RolesRepository : IRolesRepository
    {

        private readonly IDbSettings _dbSettings;

        public RolesRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task<IEnumerable<StandardUserRole>> GetUserRolesAsync(
            Guid userId,
            bool onlyStatic = false,
            bool onlyDynamic = false,
            bool onlyActive = true)
        {
            var Command = $"[Reading.UserData].[GetUserRoles]";

            var param = new { UserId = userId, OnlyStatic = onlyStatic, OnlyDynamic = onlyDynamic, OnlyActive = onlyActive };

            using var connection = _dbSettings.CreateConnection();
            return await connection.QueryAsync<StandardUserRole>
                (Command, param, commandType: CommandType.StoredProcedure);
        }


        public async Task<byte> GetRoleIdByNameAsync(string roleName)
        {
            var Command = $"[Reading.UserData].[GetRoleIdByName]";
            var param = new { RoleName = roleName };
            using var connection = _dbSettings.CreateConnection();
            return await connection.QuerySingleAsync<byte>
                (Command, param, commandType: CommandType.StoredProcedure);

        }
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            //not implemented yet
            var Command = $"[Reading.UserData].[GetRoleByName]";
            var param = new { RoleName = roleName };
            using var connection = _dbSettings.CreateConnection();
            return await connection.QuerySingleAsync<Role>
                (Command, param, commandType: CommandType.StoredProcedure);

        }




    }
}

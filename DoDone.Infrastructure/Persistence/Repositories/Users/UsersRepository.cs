using Dapper;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Users;
using DoDone.Infrastructure;
using DoDone.Infrastructure.Persistence.DbSettings;
using Microsoft.Extensions.Options;
using System.Data;

namespace DoDone.Infrastructure.Persistence.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbSettings _dbSettings;


        public UsersRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }



        public async Task AddUserAsync(User user, IDbConnection dbConnection = null, IDbTransaction transaction = null)
        {
            var command = "[Writing.UserData].AddUser";
            var externalConnection = dbConnection != null;

            var connection = dbConnection ?? _dbSettings.CreateConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var sqlparams = new
                {
                    user.Id,
                    user.FullName,
                    user.ShowName,
                    user.Email,
                    user.IsVerified,
                    user._passwordHash,
                    user.CreatedAt,
                    user.ProfilePhotoLink,
                    user.Is2FAEnabled
                };

                if (transaction != null && transaction.Connection != connection)
                    throw new InvalidOperationException("Transaction must be associated with the same connection.");

                await connection.ExecuteAsync(
                    sql: command,
                    param: sqlparams,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );
            }
            finally
            {
                if (!externalConnection)
                    connection.Dispose();
            }
        }



        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var Command = "[Reading.UserData].ExistsUserByEmail";

            using var connection = _dbSettings.CreateConnection();

            return await connection.ExecuteScalarAsync<bool>(
                Command,
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        // 3️⃣ GetByEmail
        public async Task<User?> GetByEmailAsync(string email)
        {
            var Command = "[Reading.UserData].GetUserByEmail";

            using var connection = _dbSettings.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                Command,
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var command = "[Reading.UserData].GetUserById";

            using var connection = _dbSettings.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                command,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task Update(User user, IDbConnection dbConnection = null, IDbTransaction transaction = null)
        {
            var command = "[Writing.UserData].UpdateUserData";
            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var sqlparams = new
                {
                    user.Id,
                    user.FullName,
                    user.ShowName,
                    user.Email,
                    user.IsVerified,
                    user._passwordHash,
                    user.ProfilePhotoLink,
                    user.Is2FAEnabled
                };

                if (transaction != null && transaction.Connection != connection)
                    throw new InvalidOperationException("Transaction must be associated with the same connection.");

                await connection.ExecuteAsync(
                    sql: command,
                    param: sqlparams,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );
            }
            finally
            {
                if (!externalConnection)
                    connection.Dispose();
            }
        }


        public async Task<EmployeeProfileResult?> GetEmployeeProfileById(Guid userId)
        {
            var Command = "[Reading.UserData].GetEmployeeProfile";

            using var connection = _dbSettings.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<EmployeeProfileResult>(
                Command,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<EmployeeProfileResult>> GetEmployees(int BageNum = 0, int BageSize = 12)
        {
            var command = "[Reading.UserData].GetEmployees";
            var parameters = new { BageNum, BageSize };

            using var conn = _dbSettings.CreateConnection();
            var result = await conn.QueryAsync<EmployeeProfileResult>(
                sql: command,
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }


    }
}

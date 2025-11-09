using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Users;
using DoDone.Infrastructure.Persistence.DbSettings;
using Microsoft.Extensions.Options;
using System.Data;

namespace DoDone.Infrastructure.Persistence.Repositories.Users
{
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly IDbSettings _dbSettings;

        public UserTokensRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }



        public async Task AddAsync(UserToken token, IDbConnection dbConnection = null, IDbTransaction transaction = null)
        {
            var command = "[Writing.UserData].AddUserToken";
            var parameters = new
            {
                token.Id,
                token.UserId,
                token.Type,
                token.Token,
                token.CreatedAt,
                token.IsUsed,
                token.UsedAt
            };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                if (transaction != null && transaction.Connection != connection)
                    throw new InvalidOperationException("Transaction must be associated with the same connection.");

                await connection.ExecuteAsync(
                    sql: command,
                    param: parameters,
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

        public async Task<int> UseTokenAsync(Guid userId, string token, string type, IDbConnection dbConnection = null, IDbTransaction transaction = null)
        {
            var command = "[Reading.UserData].UseUserToken";
            var parameters = new
            {
                UserId = userId,
                Token = token,
                Type = type
            };

            var externalConnection = dbConnection != null;
            var connection = dbConnection ?? _dbSettings.CreateConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                if (transaction != null && transaction.Connection != connection)
                    throw new InvalidOperationException("Transaction must be associated with the same connection.");

                return await connection.ExecuteScalarAsync<int>(
                    sql: command,
                    param: parameters,
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
    }


}

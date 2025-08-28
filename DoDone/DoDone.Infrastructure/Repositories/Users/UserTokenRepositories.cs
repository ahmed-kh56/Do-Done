using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Users;
using HealLink.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Repositories.Users
{
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly string _connectionString;
        public UserTokensRepository(IOptions<DbSettings> options)
        {
            _connectionString = options.Value.ConnectionSting;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task AddAsync(UserToken token)
        {
            using var connection = CreateConnection();

            var parameters = new
            {
                Id = token.Id,
                UserId = token.UserId,
                Type = token.Type,
                Token = token.Token,
                CreatedAt = token.CreatedAt,
                IsUsed = token.IsUsed,
                UsedAt = token.UsedAt
            };

            await connection.ExecuteAsync(
                "Writing.AddUserToken",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<int> UseTokenAsync(Guid userId, string token, string type)
        {
            using var connection = CreateConnection();

            var parameters = new
            {
                UserId = userId,
                Token = token,
                Type = type
            };


            return await connection.ExecuteScalarAsync<int>(
                "Reading.UsingUserToken",
                parameters,
                commandType: CommandType.StoredProcedure
            ); ;
        }


    }

}

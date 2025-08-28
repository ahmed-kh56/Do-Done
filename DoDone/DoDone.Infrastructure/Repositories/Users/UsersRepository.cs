
using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;

namespace HealLink.Infrastructure.Persistence.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;


        public UsersRepository(IOptions<DbSettings> options)
        {
            _connectionString = options.Value.ConnectionSting;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task AddUserAsync(User user)
        {
            var procedureName = "Writing.AddUser";

            using var connection = CreateConnection();

            await connection.ExecuteAsync(
                procedureName,
                new
                {
                    Id = user.Id, 
                    FullName = user.FullName,
                    ShowName = user.ShowName,
                    Email = user.Email,
                    IsVerified = user.IsVerified,
                    _passwordHash = user._passwordHash,
                    CreatedAt = user.CreatedAt,
                    ProfilePhotoLink = user.ProfilePhotoLink,
                    Is2FAEnabled = user.Is2FAEnabled
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var procedureName = "Reading.ExistsUserByEmail";

            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<bool>(
                procedureName,
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        // 3️⃣ GetByEmail
        public async Task<User?> GetByEmailAsync(string email)
        {
            var procedureName = "Reading.GetUserByEmail";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                procedureName,
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        public Task<User?> GetByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async void Update(User user)
        {
            var procedureName = "Writing.UpdateUserData";

            using var connection = CreateConnection();

            await connection.ExecuteAsync(
                procedureName,
                new
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    ShowName = user.ShowName,
                    Email = user.Email,
                    IsVerified = user.IsVerified,
                    _passwordHash = user._passwordHash,
                    ProfilePhotoLink = user.ProfilePhotoLink,
                    Is2FAEnabled = user.Is2FAEnabled
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

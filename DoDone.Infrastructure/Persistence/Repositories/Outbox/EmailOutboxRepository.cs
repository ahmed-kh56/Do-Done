using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Outbox;
using DoDone.Infrastructure.Persistence.DbSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Persistence.Repositories.Outbox
{
    public class EmailOutboxRepository : IEmailOutboxRepository
    {
        private readonly IDbSettings _dbSettings;

        public EmailOutboxRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task AddAsync(
            EmailOutbox email,
            IDbConnection? dbConnection = null,
            IDbTransaction? dbTransaction = null)
        {
            var command = "[Writing.UserData].[AddEmailOutbox]";
            var parameters = new
            {
                email.Id,
                email.UserId,
                email.UserName,
                email.UserEmail,
                email.EmailType,
                email.Token
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


        public async Task<IEnumerable<PendingEmail>> GetPendingAsync(int take = 50)
        {
            using var _dbConnection = _dbSettings.CreateConnection();
            return await _dbConnection.QueryAsync<PendingEmail>(
                "[Reading.UserData].[GetPendingEmails]",
                new { Take = take },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task UpdateAsync(EmailOutbox email)
        {
            using var connection = _dbSettings.CreateConnection();

            var command = "[Writing.UserData].[UpdateEmailOutbox]";
            var parameters = new
            {
                email.Id,
                email.IsSent,
                email.SentAt,
                email.ErrorMessage,
                email.RetryCount
            };

            await connection.ExecuteAsync(
                command,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}

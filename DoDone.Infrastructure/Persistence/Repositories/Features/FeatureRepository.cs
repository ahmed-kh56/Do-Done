using Dapper;
using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Infrastructure.Persistence.DbSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Persistence.Repositories.Features
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly IDbSettings _dbSettings;
        public FeatureRepository(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }
        public async Task UnassaignAllAsync(
            Guid? featureId = null,
            Guid? purId = null,
            Guid? projectId = null,
            Guid? userId = null,
            bool onlyNotCompleted = true,
            IDbConnection dbConnection = null,
            IDbTransaction dbTransaction = null)
        {
            var command = "[Writing.FeatureTasks].[UnassaignAll]";
            var parameters = new
            {
                featureId,
                purId,
                projectId,
                userId,
                onlyNotCompleted
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
    }
}

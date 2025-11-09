using Dapper;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Infrastructure.Persistence.DbSettings;
using ErrorOr;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;


namespace DoDone.Infrastructure.Services.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly IDbSettings _dbSettings;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public UnitOfWork(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection is null)
                {
                    _connection = _dbSettings.CreateConnection();
                }
                return _connection;
            }
        }
        public IDbTransaction? Transaction => _transaction;



        public async Task StartTransactionAsync()
        {
            if (_connection is null)
            {
                _connection = _dbSettings.CreateConnection();
            }

            if (_connection.State != ConnectionState.Open)
            {
                if (_connection is DbConnection dbConn)
                {
                    await dbConn.OpenAsync();
                }

            }

            _transaction = _connection.BeginTransaction();
        }


        public Task CommitTransactionAsync()
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction to commit.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public Task RollbackTransactionAsync()
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction to rollback.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            _transaction?.Dispose();
            if (_connection is IAsyncDisposable asyncDisposableConn)
            {
                return asyncDisposableConn.DisposeAsync();
            }
            else
            {
                _connection?.Dispose();
                return ValueTask.CompletedTask;
            }
        }




        public async Task ExecuteInTransactionAsync
            (Func<IDbConnection, IDbTransaction, Task> action)
        {
            try
            {
                await StartTransactionAsync();

                await action(_connection, _transaction);

                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Type: {ex.GetType()}");
                Console.WriteLine($"Massage: {ex.Message}");
                Console.WriteLine($"Source: {ex.Source}");
                await RollbackTransactionAsync();
            }
        }

        public async Task ExecuteInTransactionAsync
            (params Func<IDbConnection, IDbTransaction, Task>[] actions)
        {
            try
            {
                await StartTransactionAsync();

                foreach (var action in actions)
                {
                    await action(_connection,_transaction );
                }

                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Type: {ex.GetType()}");
                Console.WriteLine($"Massage: {ex.Message}");
                Console.WriteLine($"Source: {ex.Source}");
                await RollbackTransactionAsync();
            }
        }


    }
}

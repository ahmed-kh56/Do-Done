
using ErrorOr;
using System.Data;

namespace DoDone.Application.Common.Interfaces.Services
{
    public interface IUnitOfWork
    {
        IDbTransaction Transaction { get; }
        IDbConnection Connection { get; }

        Task StartTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task ExecuteInTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action);
        Task ExecuteInTransactionAsync(params Func<IDbConnection, IDbTransaction, Task>[] actions);


    }
}

using DoDone.Domain.Features;
using System.Data;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface ITaskItemRepository
    {
        Task MarkAllAsAsync(
            TaskItemStatus status,
            Guid? purId = null,
            Guid? ProjectId = null,
            bool onlyIfNotDone = true,
            IDbConnection? dbConnection=null,
            IDbTransaction? dbTransaction=null);

    }
}

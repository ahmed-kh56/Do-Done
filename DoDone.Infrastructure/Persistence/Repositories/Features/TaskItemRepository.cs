using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Features;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Persistence.Repositories.Features
{
    public class TaskItemRepository : ITaskItemRepository
    {
        public Task MarkAllAsAsync(TaskItemStatus status, Guid? purId = null, Guid? ProjectId = null, bool onlyIfNotDone = true, IDbConnection? dbConnection = null, IDbTransaction? dbTransaction = null)
        {
            throw new NotImplementedException();
        }
    }
}

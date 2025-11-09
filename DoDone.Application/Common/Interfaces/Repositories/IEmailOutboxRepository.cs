using DoDone.Domain.Outbox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IEmailOutboxRepository
    {
        Task AddAsync(EmailOutbox email, IDbConnection? dbConnection = null, IDbTransaction? dbTransaction = null);
        Task<IEnumerable<PendingEmail>> GetPendingAsync(int take = 50);
        Task UpdateAsync(EmailOutbox email);

    }

}

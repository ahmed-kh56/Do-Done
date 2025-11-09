using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Persistence.Repositories.Roles
{
    public class UserRolesRepository : IUserRolesRepository
    {
        public Task<int> AddAsync(UserRoles userRole, IDbConnection conn = null, IDbTransaction tran = null)
        {
            throw new NotImplementedException();
        }
    }
}

using DoDone.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IUserRolesRepository
    {
        Task<int> AddAsync(UserRoles userRole, IDbConnection conn=null, IDbTransaction tran=null);
    }
}

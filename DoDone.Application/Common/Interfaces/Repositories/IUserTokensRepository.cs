using DoDone.Domain.Users;
using System.Data;


namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IUserTokensRepository
    {




        public  Task AddAsync(UserToken token,
            IDbConnection dbConnection = null, IDbTransaction transaction = null);

        public Task<int> UseTokenAsync(
            Guid userId, string token, string type,
            IDbConnection dbConnection = null, IDbTransaction transaction = null);

    }
}

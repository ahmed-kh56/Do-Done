using DoDone.Domain.Users;


namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IUserTokensRepository
    {

        public  Task AddAsync(UserToken token);

        public Task<int> UseTokenAsync(Guid userId, string token, string type);

    }
}

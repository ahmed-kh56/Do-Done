using DoDone.Domain.Users;

namespace DoDone.Application.Common.Interfaces.Repositories;

public interface IUsersRepository
{
    Task AddUserAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid userId);
    void Update(User user);
}
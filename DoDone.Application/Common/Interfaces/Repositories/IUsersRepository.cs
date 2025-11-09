using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Domain.Users;
using System.Data;

namespace DoDone.Application.Common.Interfaces.Repositories;

public interface IUsersRepository
{
    // Reading operations
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid userId);
    // Writing operations
    Task AddUserAsync(User user, IDbConnection dbConnection = null, IDbTransaction transaction = null);
    Task Update(User user, IDbConnection dbConnection = null, IDbTransaction transaction = null);
    Task<EmployeeProfileResult?> GetEmployeeProfileById(Guid userId);
    Task<IEnumerable<EmployeeProfileResult>> GetEmployees(int BageNum = 0, int BageSize = 12);
}
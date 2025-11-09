using DoDone.Application.Common.Dtos.Roles;
using DoDone.Domain.Roles;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<byte> GetRoleIdByNameAsync(string roleName);
        Task<IEnumerable<StandardUserRole>> GetUserRolesAsync(
            Guid userId, bool onlyStatic = false, bool onlyDynamic = false, bool onlyActive = true);
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}

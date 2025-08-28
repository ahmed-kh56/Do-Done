using DoDone.Application.Queries.Authentication;
using DoDone.Domain.Roles;

namespace DoDone.Application.Common.Interfaces.Repositories;

public interface IProjectUserRolesRepository
{
    Task<IEnumerable<UserProjectRoleDto>?> GetActiveUserRolesAsync(Guid userId, bool onlyActive= false);
}
using DoDone.Application.Queries.Authentication;
using DoDone.Domain.Roles;
using DoDone.Domain.Users;

namespace DoDone.Application.Common.Interfaces.Service;

public interface ITokenGenerator
{
    UserToken GenerateUserTokens(User user, TokenTypes type);
    string GenerateJwtToken(User user,List<UserProjectRoleDto>? roles=null);

}
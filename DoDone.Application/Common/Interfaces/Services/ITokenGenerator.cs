using DoDone.Application.Common.Dtos.Roles;
using DoDone.Domain.Users;

namespace DoDone.Application.Common.Interfaces.Service;

public interface ITokenGenerator
{
    UserToken GenerateUserTokens(User user, EmailTemplateType type);
    string GenerateJwtToken(User user,List<StandardUserRole>? roles=null);

}
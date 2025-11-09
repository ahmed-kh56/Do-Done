using System.Security.Claims;

namespace DoDone.Application.Common.Models;

public record CurrentUser(
    Guid Id,
    string FullName,
    string Email,
    List<string> Roles,
    List<string> DynamicRoles,
    List<Claim> Claims);
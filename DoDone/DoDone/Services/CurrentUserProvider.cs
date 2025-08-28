using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Models;
using ErrorOr;

namespace DoDone.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ErrorOr<CurrentUser> GetCurrentUser()
        {
            try
            {
                var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
                var fullName = _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value;
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
                var roles = _httpContextAccessor.HttpContext?.User?.FindAll("roles")?.Select(r => r.Value).ToList() ?? new List<string>();
                var dynamicRoles = _httpContextAccessor.HttpContext?.User?.FindAll("dynamicroles")?.Select(r => r.Value).ToList() ?? new List<string>();

                var currentUser = new CurrentUser(
                    Guid.Parse(userIdString),
                    fullName,
                    email,
                    roles,
                    dynamicRoles);
                return currentUser;
            }
            catch (Exception ex)
            {
                return Error.Failure(description: "Failed to get current user from context: " + ex.Message);
            }

        }
    }
}

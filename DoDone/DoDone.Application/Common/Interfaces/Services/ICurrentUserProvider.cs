using ErrorOr;
using DoDone.Application.Common.Models;

namespace DoDone.Application.Common.Interfaces.Service;

public interface ICurrentUserProvider
{
    ErrorOr<CurrentUser> GetCurrentUser();
}
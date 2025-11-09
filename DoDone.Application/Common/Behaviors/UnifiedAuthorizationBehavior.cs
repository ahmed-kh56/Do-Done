using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Models;
using ErrorOr;
using MediatR;
using System.Reflection;

namespace DoDone.Application.Common.Behaviors;

public class UnifiedAuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ICurrentUserProvider _currentUserProvider;

    public UnifiedAuthorizationBehavior(ICurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requiredstaticRole = request.GetType().GetCustomAttributes<AuthorizeAttribute>().FirstOrDefault();
        if (requiredstaticRole is null)
            return await next();

        var currentUserResult = _currentUserProvider.GetCurrentUser();
        if (currentUserResult.IsError)
            return (dynamic)currentUserResult.Errors.First();

        var currentUser = currentUserResult.Value;

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ");
        foreach (var claim in currentUser.Claims)
            Console.WriteLine(claim.Type+"              :"+claim.Value);
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ");



        var staticOk = CheckStaticAuthorization(request, currentUser,requiredstaticRole);
        var dynamicOk = CheckDynamicAuthorization(request, currentUser);

        var logicAttr = request.GetType().GetCustomAttribute<AuthorizationLogicAttribute>();
        var mode = logicAttr?.Mode ?? AuthorizationMode.Or;

        bool authorized =
            mode == AuthorizationMode.And
                ? staticOk && dynamicOk
                : staticOk || dynamicOk;

        if (!authorized)
        {
            return (dynamic)Error.Unauthorized(
                description: $"Authorization failed. Mode={mode}, Static={staticOk}, Dynamic={dynamicOk}");
        }

        return await next();
    }

    private bool CheckStaticAuthorization<TRequest>(
        TRequest request,
        CurrentUser currentUser,
        AuthorizeAttribute requiredstaticRole)
    {
        if (requiredstaticRole is null) return true;
        return currentUser.Roles.Any(role => role == requiredstaticRole.Role);
    }



    private bool CheckDynamicAuthorization<TRequest>(TRequest request, CurrentUser currentUser)
    {
        if (request is not IProjectAuthorizationRequest projectRequest)
            return true;

        if (projectRequest.ProjectId == Guid.Empty)
            return false;

        if (projectRequest.RequiredDynamicRoles is null || !projectRequest.RequiredDynamicRoles.Any())
            return true;

        foreach (var requirement in projectRequest.RequiredDynamicRoles)
        {
            var key = $"{requirement.Role}:{projectRequest.ProjectId:N}";

            if (requirement.RequiresFeature)
            {
                if (!projectRequest.FeatureId.HasValue || projectRequest.FeatureId.Value == Guid.Empty)
                    continue;

                key += $":{projectRequest.FeatureId.Value:N}";
            }
            else
            {
                key += ":";
            }

            if (currentUser.DynamicRoles.Contains(key))
                return true;
        }

        return false;
    }


}

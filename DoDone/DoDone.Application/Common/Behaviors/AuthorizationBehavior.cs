using System.Reflection;
using ErrorOr;
using DoDone.Application.Common.Authorization;
using MediatR;
using DoDone.Application.Common.Interfaces.Service;

namespace DoDone.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider _currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var currentUserRuselt = _currentUserProvider.GetCurrentUser();
        if (currentUserRuselt.IsError)
        {
            return (dynamic)currentUserRuselt.Errors.First();
        }
        var currentUser = currentUserRuselt.Value;

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Role?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentUser.Roles).Count()>0)
        {
            return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
        }

        return await next();
    }

}


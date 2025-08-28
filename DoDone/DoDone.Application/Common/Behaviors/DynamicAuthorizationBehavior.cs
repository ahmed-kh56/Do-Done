using DoDone.Application.Common.Authorization;
using DoDone.Application.Common.Interfaces.Service;
using ErrorOr;
using MediatR;
using System.Reflection;

namespace DoDone.Application.Common.Behaviors
{
    public class DynamicAuthorizationBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr<TResponse>
    {
        public DynamicAuthorizationBehavior(ICurrentUserProvider currentUserProvider) 
        {
            _currentUserProvider = currentUserProvider;
        }
        private readonly ICurrentUserProvider _currentUserProvider;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var requredDynamicRoles = request.GetType()
                .GetCustomAttributes<DynamicAuthorizeAttribute>()
                .Select(attr => attr.ToString())
                .ToList();
            if (requredDynamicRoles.Count==0)
            {
                return await next();
            }

            var currentUserResult = _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return (dynamic)currentUserResult.Errors.First();
            }
            var currentUser = currentUserResult.Value;
            var dynamicRoles = currentUser.DynamicRoles;

            if (!(requredDynamicRoles.Except(currentUser.DynamicRoles).Count() ==0))
            {
                return (dynamic)Error.Unauthorized(description: "User requeres dynamic role for this action");
            }

            return await next();
        }
    }
}

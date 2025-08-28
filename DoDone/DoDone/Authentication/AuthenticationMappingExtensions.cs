using DoDone.Application.Authentication.Common;
using DoDone.Application.Commands.Authentication.Register;
using DoDone.Application.Queries.Authentication;

namespace DoDone.Authentication
{
    // RegisterMappingExtensions.cs
    public static class AuthenticationMappingExtensions
    {
        public static RegisterCommand ToCommand(this RegisterRequest request)
            => new(request.FullName, request.NameToShow, request.Email, request.Password);



        public static AuthResponse FromAuthenticationResult(this AuthenticationResult authResult)
            => new(
                authResult.User.Id,
                authResult.User.FullName,
                authResult.User.ShowName,
                authResult.User.Email,
                authResult.Token);



    }

}

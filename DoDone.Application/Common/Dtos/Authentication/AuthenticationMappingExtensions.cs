using DoDone.Application.Commands.Authentication.Register;
using DoDone.Application.Common.Dtos.UserDtos;
using DoDone.Application.Queries.Authentication.Login;

namespace DoDone.Application.Common.Dtos.Authentication
{
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
        public static LoginQuery ToLoginQuery(this LoginRequest request)
            => new(request.Email, request.Password);



    }

}

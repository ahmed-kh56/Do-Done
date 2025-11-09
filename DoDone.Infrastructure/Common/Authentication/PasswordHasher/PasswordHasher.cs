using ErrorOr;
using DoDone.Domain.Common;
using System.Text.RegularExpressions;
namespace DoDone.Infrastructure.Common.Authentication.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {

        public ErrorOr<string> HashPassword(string password)
            => (password.Length < 9 ||
                !password.Any(char.IsLetter) ||
                !password.Any(char.IsDigit)) 
                ?Error.Validation(description: "Password is too weak") 
                : BCrypt.Net.BCrypt.EnhancedHashPassword(password);


        public bool IsCorrectPassword(string password, string hash)
          => BCrypt.Net.BCrypt.EnhancedVerify(password,hash);
        

    }
}

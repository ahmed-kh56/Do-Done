using DoDone.Application.Common.Dtos.Roles;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DoDone.Infrastructure.Common.Authentication.TokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {


        private readonly JwtSettings _jwtSettings;

        public TokenGenerator(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }


        public string GenerateJwtToken(User user, List<StandardUserRole>? roles = null)
        {
            if (_jwtSettings is null)
                throw new Exception("JwtSettings section not found in configuration!");


            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ");
            Console.WriteLine($"Issuer: {_jwtSettings?.Issuer}");
            Console.WriteLine($"Key: {_jwtSettings?.Key ?? "NULL"}");
            Console.WriteLine($"Audience: {_jwtSettings?.Audience}");

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ");




            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)); // Replace with your actual secret key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("contactinfo", user.Email),
                new Claim("name", user.FullName)
            };
            var dynamicroles = roles?.Where(r => r.ProjectId is not null).ToList();
            var staticRoles = roles?.Where(r => r.ProjectId is null).ToList();
            foreach (var role in staticRoles ?? Enumerable.Empty<StandardUserRole>())
            {
                claims.Add(new Claim("staticroles", role.StaticToString()));
            }
            foreach (var role in dynamicroles ?? Enumerable.Empty<StandardUserRole>())
            {
                claims.Add(new Claim("dynamicroles", role.DynamicToString()));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public UserToken GenerateUserTokens(User user, EmailTemplateType tokenType)
        {
            string token;

            switch (tokenType)
            {
                case EmailTemplateType.refreshToken:
                    token = GenerateSecureRandomString(128);
                    break;

                case EmailTemplateType.emailConfirmation:
                    token = GenerateSecureRandomString(6);
                    break;

                case EmailTemplateType.passwordReset:
                    token = GenerateSecureRandomString(6);
                    break;

                case EmailTemplateType.TwoFA:
                    token = GenerateNumericCode(6);
                    break;

                default:
                    throw new ArgumentException("Unsupported token type");
            }

            return new UserToken(user.Id, tokenType, token);

        }



        public static string GenerateSecureRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            var result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }

        private string GenerateNumericCode(int length)
        {
            var rng = new Random();
            return rng.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
        }

    }
}

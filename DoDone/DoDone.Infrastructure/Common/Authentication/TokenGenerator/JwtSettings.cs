using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Infrastructure.Common.Authentication.TokenGenerator
{
    public class JwtSettings
    {
        public const string Section = "JwtSettings";
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string key { get; set; } = null!;

        public int TokenExpirationInMinutes { get; set; }
    }
}

using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Domain.Common;
using DoDone.Infrastructure.Common.Authentication.PasswordHasher;
using DoDone.Infrastructure.Common.Authentication.TokenGenerator;
using DoDone.Infrastructure.Repositories.Roles;
using DoDone.Infrastructure.Repositories.Users;
using DoDone.Infrastructure.Services.Email;
using HealLink.Infrastructure.Persistence.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace HealLink.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAuthentication(configuration)
            .AddPersistence(configuration)
            .AddServices(configuration);
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection("DbSettings"));

        services.AddScoped<IUserTokensRepository, UserTokensRepository>();
        services.AddScoped<IProjectUserRolesRepository,ProjectUserRolesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(JwtSettings.Section);
        services.Configure<JwtSettings>(jwtSection);
        //services.Configure<JwtSettings>(configuration.GetSection("JwtSection"));

        var jwtSettings = jwtSection.Get<JwtSettings>();

        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.key)),
            });

        return services;
    }

}
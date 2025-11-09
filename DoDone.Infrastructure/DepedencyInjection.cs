using DoDone.Application.Common.Interfaces.Repositories;
using DoDone.Application.Common.Interfaces.Service;
using DoDone.Application.Common.Interfaces.Services;
using DoDone.Domain.Common;
using DoDone.Infrastructure.Common.Authentication.PasswordHasher;
using DoDone.Infrastructure.Common.Authentication.TokenGenerator;
using DoDone.Infrastructure.Persistence.DbSettings;
using DoDone.Infrastructure.Persistence.Repositories.Features;
using DoDone.Infrastructure.Persistence.Repositories.Outbox;
using DoDone.Infrastructure.Persistence.Repositories.Projects;
using DoDone.Infrastructure.Persistence.Repositories.Roles;
using DoDone.Infrastructure.Persistence.Repositories.Users;
using DoDone.Infrastructure.Services.BackgroundEmailOutbox;
using DoDone.Infrastructure.Services.Email;
using DoDone.Infrastructure.Services.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace DoDone.Infrastructure;

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
        services.Configure<DbSettings>(
            configuration.GetSection("DbSettings")
        );

        services.AddSingleton<IDbSettings>(sp =>
            sp.GetRequiredService<IOptions<DbSettings>>().Value
        );

        services.AddScoped<IUserTokensRepository, UserTokensRepository>();
        services.AddScoped<IProjectRepository,ProjectRepository>();
        services.AddScoped<IRolesRepository,RolesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailOutboxRepository, EmailOutboxRepository>();
        services.AddScoped<IFeatureRepository,FeatureRepository>();
        services.AddScoped<IPURRepository,PURRepository>();
        services.AddScoped<IUserRolesRepository,UserRolesRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddHostedService<EmailOutboxBackGroundService>();
        return services;
    }


    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                };
            });

        return services;
    }

}
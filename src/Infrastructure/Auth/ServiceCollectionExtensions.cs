

using Application.Common.Exceptions;
using Infrastructure.Auth.Jwt;
using Infrastructure.Identity;
using Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth;

internal static class ServiceCollectionExtensions
{

    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        return services
                    .AddIdentity()
                    .AddJwtAuth(config);
    }
    /// <summary>
    /// JWT验证授权
    /// </summary>
    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection($"AuthSettings:{nameof(JwtSettings)}"));
        var jwtSettings = config.GetSection($"AuthSettings:{nameof(JwtSettings)}").Get<JwtSettings>();
        if (string.IsNullOrWhiteSpace(jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");      
        }
        byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);
        return services.AddAuthentication(authentication =>
        {
            authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            bearer.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        throw new UnauthorizedException("Authentication Failed.");
                    }

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["Authorization"];

                    if (!string.IsNullOrEmpty(accessToken) &&
                        context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        })
         .Services;
    }
    /// <summary>
    /// 添加用户标识
    /// </summary>
    public static IServiceCollection AddIdentity(
      this IServiceCollection services)
    {
        return services
                    .AddIdentity<ApplicationUser, ApplicationRole>(option =>
                    {
                        option.Password.RequiredLength = 6;
                        option.Password.RequireDigit = false;
                        option.Password.RequireLowercase = false;
                        option.Password.RequireNonAlphanumeric = false;
                        option.Password.RequireUppercase = false;
                        option.User.RequireUniqueEmail = true;
                    })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders().Services;
    }
}

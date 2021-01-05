using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Vitamin.Authentication
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static void AddAuthenticaton(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                             {
                                 options.AccessDeniedPath = "/admin/accessdenied";
                                 options.LoginPath = "/admin/signin";
                                 options.LogoutPath = "/admin/signout";
                             });

        }
    }
}

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vitamin.Authentication
{
    public static class AuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// 验证方式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticaton(this IServiceCollection services, IConfiguration configuration)
        {
            //获取appsettings 中的登录配置
            var authentication = new AuthenticationSettings();
            configuration.Bind(nameof(Authentication), authentication);
            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(Authentication)));

            switch (authentication.Provider)
            {
                case AuthenticationProvider.AzureAD:
                    break;
                case AuthenticationProvider.Local:
                    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                            {
                                options.AccessDeniedPath = "/admin/accessdenied";
                                options.LoginPath = "/admin/signin";
                                options.LogoutPath = "/admin/signout";
                            });
                    break;
                case AuthenticationProvider.None:
                    break;
                default:
                    var msg = $"Provider {authentication.Provider} is not supported.";
                    throw new NotSupportedException(msg);
            }
            }
    }
}

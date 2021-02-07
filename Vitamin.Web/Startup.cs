using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vitamin.Data.Model;
using Vitamin.Moled.Settings;
using Vitamin.Web.Configuration;

namespace Vitamin.Web
{
    public class Startup
    {
        private readonly IConfigurationSection _appSettings;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration,IWebHostEnvironment env) 
        {
            _configuration = configuration;
            _environment = env;
            _appSettings = _configuration.GetSection(nameof(AppSettings));
        }

        //依赖注入 定义应用使用的服务
        public void ConfigureServices(IServiceCollection services)
        {
            //添加网站配置
            services.AddVitaminConfiguration(_appSettings);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                             {
                                 options.AccessDeniedPath = "/admin/accessdenied";
                                 options.LoginPath = "/admin/signin";
                                 options.LogoutPath = "/admin/signout";
                             });

            services.AddMvc(options =>
                            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            //批量注入服务
            services.AddVitaminServices();
            //数据连接
            services.AddDataStorage(_configuration.GetConnectionString(Constants.DbConnectionName));
        }

        // 中间件
        public void Configure(
            IApplicationBuilder app)
        {
           // app.UseMiddleware<FirstRunMiddleware>();
           //生产环境
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
                app.UseExceptionHandler("/error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }    
            //路由
            app.UseRouting();
            //使用静态文件
            app.UseStaticFiles();
            //身法验证
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllerRoute(
                 "default",
                 "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vitamin.Authentication;
using Vitamin.Web.Configuration;

namespace Vitamin.Web
{
    public class Startup
    {
        private ILogger<Startup> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration,IWebHostEnvironment env) 
        {
            _configuration = configuration;
            _environment = env;
        }

        //依赖注入 定义应用使用的服务
        public void ConfigureServices(IServiceCollection services)
        {
            //将appsettings.json中的配置绑定到xxx的实例，并完成依赖注入。
            services.AddOptions();
            //验证方式
            services.AddAuthenticaton(_configuration);
            services.AddMvc(options =>
                            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //批量注入服务
            services.AddVitaminServices();
            //数据连接
            services.AddDataStorage(_configuration);
        }

        // 中间件
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
           // app.UseMiddleware<FirstRunMiddleware>();
           //生产环境
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            //使用静态文件
            app.UseStaticFiles();
            //路由
            app.UseRouting();
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

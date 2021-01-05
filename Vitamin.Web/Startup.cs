using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
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
            _environment = env; ;
        }

        //依赖注入 定义应用使用的服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticaton(_configuration);
         
            services.AddMvc(options =>
                      options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
                      .AddViewLocalization()
                      .AddDataAnnotationsLocalization();
           
            services.AddVitaminServices();
        }

        // 中间件
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
                app.UseExceptionHandler("/error");
                //Https
                app.UseHttpsRedirection();
                app.UseHsts();
            }
            app.UseStaticFiles();

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

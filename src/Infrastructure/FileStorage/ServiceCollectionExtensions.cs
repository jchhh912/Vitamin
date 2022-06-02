
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Infrastructure.FileStorage;

public static class ServiceCollectionExtensions
{
    public static IApplicationBuilder UseFileStorege(this IApplicationBuilder app)
    {
        return app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            RequestPath = new PathString("/Files")
        });
    }
}

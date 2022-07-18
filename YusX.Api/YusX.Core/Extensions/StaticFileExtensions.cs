using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 静态文件扩展
    /// </summary>
    public static class StaticFileExtensions
    {
        public static IApplicationBuilder UseStaticDefaultFile(this IApplicationBuilder app, string path)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(path, "Content")),
                RequestPath = new PathString("/Content"),
            })
            .UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(path, "fonts")),
                RequestPath = new PathString("/fonts"),
            })
            .UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(path, "Scripts")),
                RequestPath = new PathString("/Scripts"),
            })
            .UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(path, "Html")),
                RequestPath = new PathString("/Html"),
            });

            return app;
        }
    }
}

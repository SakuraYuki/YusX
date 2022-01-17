using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YusX.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // 指定Kestrel作为服务器并进行配置
                    webBuilder.UseKestrel().ConfigureKestrel(options =>
                    {
                        // 请求体限制大小10MB
                        options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
                    })
                    .UseIIS()
                    .UseStartup<Startup>();
                })
                // 配置Autofac
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }
    }
}

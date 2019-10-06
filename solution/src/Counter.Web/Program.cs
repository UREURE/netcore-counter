using Counter.Web.Entidades.Configuracion;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Net;

namespace Counter.Web
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppConfig configuracion = new AppConfig();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(Startup.APPSETTINGS, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            configurationRoot.Bind(configuracion);

            // Configurar NLog
            LogManager.LoadConfiguration("nlog.config");

            IWebHost webHost = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configurationRoot)
                .UseKestrel(options =>
                {
                    if (configuracion.Counter_Http_Int_Port.HasValue)
                        options.Listen(IPAddress.Any, configuracion.Counter_Http_Int_Port.Value);
                })
                .UseStartup<Startup>()
                .Build();
            webHost.Run();

            LogManager.Flush(TimeSpan.FromSeconds(15));
            LogManager.Shutdown();
        }
    }
}
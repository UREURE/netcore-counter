using Counter.Web.Constantes;
using Counter.Web.Entidades.Configuracion;
using Counter.Web.Loggers;
using Counter.Web.Middleware;
using Counter.Web.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace Counter.Web
{
    internal class Startup
    {
        public const string APPSETTINGS = "appsettings.json";

        private readonly AppConfig configuracion;

        public Startup()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(APPSETTINGS, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
            configuracion = new AppConfig();
            Configuration.Bind(configuracion);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<AppConfig>(Configuration);

            // Configurar Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info
                {
                    Title = "Counter",
                    Version = configuracion.Version
                });
                string xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });

            ConfigureRepositories(services);
        }

        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<ILogger, NLogLogger>();
            services.AddLogging(builder =>
            {
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });

            // Una Instancia cada vez que resulte necesaria
            services.AddTransient<ICounterRepository, CounterRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<HttpExceptionMiddleware>();
            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"{UriPath.PREFIX}/swagger/{{documentName}}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{UriPath.PREFIX}/swagger/v2/swagger.json", $"Counter {configuracion.Version}");
                c.RoutePrefix = $"{UriPath.PREFIX}/swagger";
            });
        }
    }
}
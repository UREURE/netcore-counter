﻿using Counter.Web.Constantes;
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
        public const int PUERTO_REDIS_DEFECTO = 6379;

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
                c.SwaggerDoc("counter", new Info
                {
                    Title = "Counter",
                    Version = configuracion.Counter_Version ?? "N/A"
                });
                string xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });

            ConfigureRepositories(services);
        }

        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            string redisHost = configuracion.Redis_Host ?? "localhost";
            int redisPort = configuracion.Redis_Port ?? PUERTO_REDIS_DEFECTO;
            string redisConfiguration = $"{redisHost}:{redisPort}";
            if (!string.IsNullOrEmpty(configuracion.Redis_Password))
                redisConfiguration = $"{redisConfiguration},password={configuracion.Redis_Password}";

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConfiguration;
                option.InstanceName = configuracion.Redis_Instance ?? "netcore-counter";
            });

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
                c.SwaggerEndpoint($"/{UriPath.PREFIX}/swagger/counter/swagger.json", $"Counter {configuracion.Counter_Version ?? "N/A"}");
                c.RoutePrefix = $"{UriPath.PREFIX}/swagger";
            });
        }
    }
}
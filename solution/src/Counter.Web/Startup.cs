using Counter.Web.Constantes;
using Counter.Web.Entidades.Configuracion;
using Counter.Web.Loggers;
using Counter.Web.Middleware;
using Counter.Web.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Caching.Distributed;
using Polly.CircuitBreaker;
using Polly.Registry;
using Prometheus;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Counter.Web
{
    internal class Startup
    {
        public const string APPSETTINGS = "appsettings.json";
        public const int PUERTO_REDIS_DEFECTO = 6379;
        public const int REINTENTOS_REDIS_DEFECTO = 3;

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
            string redisHost = "localhost";
            int redisPort = PUERTO_REDIS_DEFECTO;
            string redisPassword = null;
            string redisInstance = "netcore-counter";
            int redisReintentos = PUERTO_REDIS_DEFECTO;

            if (configuracion.Redis != null)
            {
                if (!string.IsNullOrEmpty(configuracion.Redis.Host))
                    redisHost = configuracion.Redis.Host;
                if (configuracion.Redis.Port.HasValue)
                    redisPort = configuracion.Redis.Port.Value;
                if (!string.IsNullOrEmpty(configuracion.Redis.Password))
                    redisPassword = configuracion.Redis.Password;
                if (!string.IsNullOrEmpty(configuracion.Redis.Instance))
                    redisInstance = configuracion.Redis.Instance;
                if (configuracion.Redis.Reintentos.HasValue)
                    redisReintentos = configuracion.Redis.Reintentos.Value;
            }
            string redisConfiguration = $"{redisHost}:{redisPort}";
            if (!string.IsNullOrEmpty(redisPassword))
                redisConfiguration = $"{redisConfiguration},password={redisPassword}";

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConfiguration;
                option.InstanceName = redisInstance;
            });

            services.AddSingleton<IAsyncCacheProvider<string>>(serviceProvider => serviceProvider.GetRequiredService<IDistributedCache>().AsAsyncCacheProvider<string>());
            services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
            {
                PolicyRegistry registry = new PolicyRegistry();

                CircuitBreakerPolicy policyCircuitBreaker = Policy
                .Handle<RedisConnectionException>()
                .CircuitBreaker(1, TimeSpan.FromMinutes(1),
                onBreak: (exception, timespan, context) =>
                {
                    serviceProvider.GetRequiredService<ILogger>().Error($"Utilizando CircuitBreaker durante {timespan.TotalMinutes} minutos tras el error: {exception}");
                },
                onReset: context =>
                {
                    serviceProvider.GetRequiredService<ILogger>().Error($"Finalizando CircuitBreaker.");
                });

                Policy policyIntentos = Policy
                .Handle<RedisConnectionException>()
                .Retry(redisReintentos, onRetry: (exception, retryCount, context) =>
                {
                    serviceProvider.GetRequiredService<ILogger>().Error($"Error en el intento {retryCount}: {exception}");
                })
                .Wrap(policyCircuitBreaker);

                registry.Add(Claves.CLAVE_POLITICA_CACHE, policyIntentos);
                return registry;
            });

            // ...
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
            app.UseMetricServer();
            app.UseMiddleware<RequestMiddleware>();
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
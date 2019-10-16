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
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Prometheus;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Counter.Web
{
    internal class Startup
    {
        public const string APPSETTINGS = "appsettings.json";
        public const string HOST_REDIS_DEFECTO = "localhost";
        public const int PUERTO_REDIS_DEFECTO = 6379;
        public const string INSTANCIA_REDIS_DEFECTO = "netcore-counter";
        public const int REINTENTOS_REDIS_DEFECTO = 3;

        public const string PROTOCOLO_NEXT_COUNTER_DEFECTO = "http";
        public const string HOST_NEXT_COUNTER_DEFECTO = "localhost";
        public const int PUERTO_NEXT_COUNTER_DEFECTO = 5000;
        public const int REINTENTOS_NEXT_COUNTER_DEFECTO = 3;

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

        public virtual void AddPolicies(IServiceCollection services)
        {
            services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
            {
                PolicyRegistry registry = new PolicyRegistry();

                Action<Exception, TimeSpan> onBreak = (exception, timespan) =>
                {
                    serviceProvider.GetRequiredService<ILogger>().Error($"Utilizando CircuitBreaker durante {timespan.TotalMinutes} minutos tras el error: {exception}");
                };
                Action onReset = () =>
                {
                    serviceProvider.GetRequiredService<ILogger>().Error($"Finalizando CircuitBreaker.");
                };
                AsyncCircuitBreakerPolicy breaker = Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1), onBreak, onReset);

                AsyncPolicy policyCache = Policy
                    .Handle<Exception>()
                    .RetryAsync(3, onRetry: (exception, retryCount) =>
                    {
                        serviceProvider.GetRequiredService<ILogger>().Warn($"Error en el intento {retryCount}: {exception}");
                    })
                    .WrapAsync(breaker);

                registry.Add(Claves.CLAVE_POLITICA_CACHE, policyCache);
                return registry;
            });
        }

        public virtual void AddRedis(IServiceCollection services, RedisConfig configuracion)
        {
            string redisHost = HOST_REDIS_DEFECTO;
            int redisPort = PUERTO_REDIS_DEFECTO;
            string redisPassword = null;
            string redisInstance = INSTANCIA_REDIS_DEFECTO;

            if (configuracion != null)
            {
                if (!string.IsNullOrEmpty(configuracion.Host))
                    redisHost = configuracion.Host;
                if (configuracion.Port.HasValue)
                    redisPort = configuracion.Port.Value;
                if (!string.IsNullOrEmpty(configuracion.Password))
                    redisPassword = configuracion.Password;
                if (!string.IsNullOrEmpty(configuracion.Instance))
                    redisInstance = configuracion.Instance;
            }
            string redisConfiguration = $"{redisHost}:{redisPort}";
            if (!string.IsNullOrEmpty(redisPassword))
                redisConfiguration = $"{redisConfiguration},password={redisPassword}";

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConfiguration;
                option.InstanceName = redisInstance;
            });
        }

        public virtual void AddNextCounterHttpClient(IServiceCollection services, NextCounterConfig configuracion)
        {
            string protocolo = PROTOCOLO_NEXT_COUNTER_DEFECTO;
            string host = HOST_NEXT_COUNTER_DEFECTO;
            int puerto = PUERTO_NEXT_COUNTER_DEFECTO;

            if (configuracion != null)
            {
                if (!string.IsNullOrEmpty(configuracion.Protocolo))
                    protocolo = configuracion.Protocolo;
                if (!string.IsNullOrEmpty(configuracion.Host))
                    host = configuracion.Host;
                if (configuracion.Port.HasValue)
                    puerto = configuracion.Port.Value;
            }
            string uri = $"{protocolo}://{host}:{puerto}/";

            services.AddHttpClient(Claves.CLAVE_CLIENTE_HTTP_NEXT_COUNTER, client =>
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        public virtual void AddRepositoryFactory(IServiceCollection services)
        {
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>((serviceProvider) =>
            {
                RepositoryFactory factoriaRepositorios = new RepositoryFactory();
                factoriaRepositorios.Add(Claves.SELECTOR_PERSISTENCIA_REDIS, serviceProvider.GetRequiredService<ICounterRedisRepository>());
                factoriaRepositorios.Add(Claves.SELECTOR_PERSISTENCIA_NEXT_COUNTER, serviceProvider.GetRequiredService<INextCounterRepository>());
                return factoriaRepositorios;
            });
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

            AddPolicies(services);

            AddRedis(services, configuracion.Redis);
            AddNextCounterHttpClient(services, configuracion.NextCounter);
            services.AddTransient<ICounterRedisRepository, CounterRedisRepository>();
            services.AddTransient<INextCounterRepository, NextCounterRepository>();
            AddRepositoryFactory(services);
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
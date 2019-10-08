using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Counter.Web.Middleware
{
    internal class RequestMiddleware
    {
        private readonly RequestDelegate next;

        public RequestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string path = httpContext.Request.Path.Value;
            string method = httpContext.Request.Method;

            Prometheus.Counter counter = Prometheus.Metrics.CreateCounter(
                "prometheus_request_total",
                "HTTP Requests Total",
                new Prometheus.CounterConfiguration { LabelNames = new[] { "path", "method", "status" } }
                );

            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception)
            {
                int statusCode = 500;
                counter.Labels(path, method, statusCode.ToString()).Inc();

                throw;
            }

            if (path != "/metrics")
            {
                counter.Labels(path, method, httpContext.Response.StatusCode.ToString()).Inc();
            }
        }
    }
}
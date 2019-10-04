using Counter.Web.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;

namespace Counter.Web.Middleware
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public HttpExceptionMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate.Invoke(context);
            }
            catch (HttpException httpException)
            {
                context.Response.StatusCode = (int)httpException.StatusCode;
                IHttpResponseFeature feature = context.Features.Get<IHttpResponseFeature>();
                feature.ReasonPhrase = httpException.Message;
            }
        }
    }
}
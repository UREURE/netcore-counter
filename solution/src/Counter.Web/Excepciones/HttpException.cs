using System;
using System.Net;

namespace Counter.Web.Excepciones
{
    internal class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode httpStatusCode)
            : base(httpStatusCode.ToString())
        {
            StatusCode = httpStatusCode;
        }
    }
}
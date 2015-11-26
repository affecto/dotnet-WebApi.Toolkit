using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;

namespace Affecto.WebApi.Toolkit
{
    public class RequestExceptionFilter : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Unknown error occured."
            };
        }

        protected static HttpResponseMessage CreateStringContentResponse(HttpStatusCode status, string content, string reason)
        {
            return new HttpResponseMessage(status)
            {
                Content = new StringContent(content),
                ReasonPhrase = reason
            };
        }

        protected static HttpResponseMessage CreateStringContentResponse(string content, string reason)
        {
            return CreateStringContentResponse(HttpStatusCode.InternalServerError, content, reason);
        }
    }
}
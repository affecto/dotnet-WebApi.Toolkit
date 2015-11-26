using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Affecto.Logging;
using Autofac.Integration.WebApi;

namespace Affecto.WebApi.Toolkit
{
    public class RequestLoggingFilter : ActionFilterAttribute, IAutofacActionFilter
    {
        private readonly ICorrelationLogger logger;
        private readonly ICorrelation correlation;

        public RequestLoggingFilter(ILoggerFactory loggerFactory, ICorrelation correlation = null)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }

            logger = loggerFactory.CreateCorrelationLogger(this);
            this.correlation = correlation;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            if (actionContext.ActionDescriptor == null)
            {
                throw new ArgumentException("HttpActionContext cannot have null ActionDescriptor.", "actionContext");
            }
            if (actionContext.Request == null)
            {
                throw new ArgumentException("Request in HttpActionContext cannot be null.", "actionContext");
            }
            if (actionContext.Request.Headers == null)
            {
                throw new ArgumentException("Headers in HttpActionContext.Request cannot be null.", "actionContext");

            }

            logger.LogVerbose(correlation, "Request received: {0}({1})", actionContext.ActionDescriptor.ActionName, actionContext.ParametersToString());
        }
    }
}
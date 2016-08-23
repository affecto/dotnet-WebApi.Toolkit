using System;
using System.Net.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Affecto.Logging;

namespace Affecto.WebApi.Toolkit
{
    public class RequestErrorLogger : ExceptionLogger
    {
        private readonly Func<HttpRequestMessage, ICorrelation> correlationFactory;
        private readonly ICorrelationLogger logger;

        public RequestErrorLogger(ILoggerFactory loggerFactory)
            : this(loggerFactory, GetCorrelationFromDependencyScope)
        {
        }

        internal RequestErrorLogger(ILoggerFactory loggerFactory, Func<HttpRequestMessage, ICorrelation> correlationFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }
            if (correlationFactory == null)
            {
                throw new ArgumentNullException("correlationFactory");
            }

            logger = loggerFactory.CreateCorrelationLogger(this);
            this.correlationFactory = correlationFactory;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            ICorrelation correlation;

            try
            {
                correlation = correlationFactory.Invoke(context.Request);
            }
            catch
            {
                correlation = null;
            }

            string parameters = context.ExceptionContext.ActionContext == null ? null : context.ExceptionContext.ActionContext.ParametersToString();
            logger.LogError(correlation, context.Exception, "Unhandled exception from request: {0}({1})", context.Request, parameters);
        }

        private static ICorrelation GetCorrelationFromDependencyScope(HttpRequestMessage request)
        {
            IDependencyScope requestScope = request.GetDependencyScope();
            return requestScope.GetService(typeof(ICorrelation)) as ICorrelation;
        }
    }
}
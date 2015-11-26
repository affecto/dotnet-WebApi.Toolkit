using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using Affecto.Logging;
using Autofac.Integration.WebApi;

namespace Affecto.WebApi.Toolkit
{
    /// <summary>
    /// Logs the information of authorized user and authentication type. Also authorization errors are logged.
    /// </summary>
    public class AuthorizationLoggingFilter : AuthorizeAttribute, IAutofacAuthorizationFilter
    {
        private readonly ICorrelationLogger logger;
        private readonly ICorrelation correlation;

        public AuthorizationLoggingFilter(ILoggerFactory loggerFactory, ICorrelation correlation = null)
        {
            
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }

            logger = loggerFactory.CreateCorrelationLogger(this);
            this.correlation = correlation;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                base.OnAuthorization(actionContext);

                if (actionContext.RequestContext.Principal == null)
                {
                    logger.LogWarning(correlation, "OnAuthorization - Principal is null.");
                }
                else if (actionContext.RequestContext.Principal.Identity == null)
                {
                    logger.LogWarning(correlation, "OnAuthorization - Identity is null.");
                }
                else
                {
                    IIdentity identity = actionContext.ControllerContext.RequestContext.Principal.Identity;

                    if (identity.IsAuthenticated)
                    {
                        logger.LogVerbose(correlation, "OnAuthorization - User name: '{0}', auth type: '{1}'.", identity.Name,
                            identity.AuthenticationType);
                    }
                    else
                    {
                        logger.LogWarning(correlation, "OnAuthorization - Identity is not authenticated. User name: '{0}', auth type: '{1}'.", identity.Name,
                            identity.AuthenticationType);
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(correlation, e, "Authorization error.");
            }
        }
    }
}
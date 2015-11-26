using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Affecto.WebApi.Toolkit.CustomRoutes
{
    public class MethodConstraintedRouteAttribute : RouteFactoryAttribute
    {
        public HttpMethod Method { get; private set; }

        public MethodConstraintedRouteAttribute(string template, HttpMethod method)
            : base(template)
        {
            Method = method;
        }

        public override IDictionary<string, object> Constraints
        {
            get { return new HttpRouteValueDictionary { { "method", new MethodConstraint(Method) } }; }
        }
    }
}
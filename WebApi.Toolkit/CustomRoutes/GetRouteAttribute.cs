using System.Net.Http;

namespace Affecto.WebApi.Toolkit.CustomRoutes
{
    public class GetRouteAttribute : MethodConstraintedRouteAttribute
    {
        public GetRouteAttribute(string template)
            : base(template, HttpMethod.Get)
        {
        }
    }
}
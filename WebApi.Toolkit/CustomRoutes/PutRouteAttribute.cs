using System.Net.Http;

namespace Affecto.WebApi.Toolkit.CustomRoutes
{
    public class PutRouteAttribute : MethodConstraintedRouteAttribute
    {
        public PutRouteAttribute(string template)
            : base(template, HttpMethod.Put)
        {
        }
    }
}
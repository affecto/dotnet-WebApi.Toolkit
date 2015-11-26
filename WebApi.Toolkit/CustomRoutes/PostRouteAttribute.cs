using System.Net.Http;

namespace Affecto.WebApi.Toolkit.CustomRoutes
{
    public class PostRouteAttribute : MethodConstraintedRouteAttribute
    {
        public PostRouteAttribute(string template)
            : base(template, HttpMethod.Post)
        {
        }
    }
}
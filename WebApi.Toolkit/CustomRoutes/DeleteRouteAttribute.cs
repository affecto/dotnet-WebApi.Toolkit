using System.Net.Http;

namespace Affecto.WebApi.Toolkit.CustomRoutes
{
    public class DeleteRouteAttribute : MethodConstraintedRouteAttribute
    {
        public DeleteRouteAttribute(string template)
            : base(template, HttpMethod.Delete)
        {
        }
    }
}
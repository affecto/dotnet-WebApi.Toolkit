using System.Web.Http;

namespace Affecto.WebApi.Toolkit
{
    public interface IHttpConfigurationProvider
    {
        void Configure(HttpConfiguration config);
    }
}
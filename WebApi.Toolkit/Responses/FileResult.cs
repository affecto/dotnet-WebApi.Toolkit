using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affecto.WebApi.Toolkit.Responses
{
    /// <summary>
    /// Http action result for returning files in a way browsers will recognize (and prompt user for open/download)
    /// </summary>
    public class FileResult : IHttpActionResult
    {
        private readonly HttpResponseMessage response;

        /// <summary>
        /// Initializes new FileResult. Tries to parse mime content type from filename.
        /// </summary>
        /// <param name="content">File content</param>
        /// <param name="filename">File name, should include extension for mime mapping to work.</param>
        public FileResult(Stream content, string filename)
        {
            response = new FileResponse(content, filename);
        }

        /// <summary>
        /// Initializes new FileResult
        /// </summary>
        /// <param name="content">File content</param>
        /// <param name="filename">File name</param>
        /// <param name="contentType">Mime content type for file</param>
        public FileResult(Stream content, string filename, string contentType)
        {
            response = new FileResponse(content, filename, contentType);
        }

        /// <summary>
        /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage"/> asynchronously.
        /// </summary>
        /// <returns>
        /// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage"/>.
        /// </returns>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(response);            
        }
    }
}

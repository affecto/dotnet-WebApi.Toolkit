using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Affecto.WebApi.Toolkit.Responses
{
    /// <summary>
    /// Http response message for returning files in a way browsers will recognize (and prompt user for open/download)
    /// </summary>
    public class FileResponse : HttpResponseMessage
    {
        /// <summary>
        /// Initializes new FileResponse. Tries to parse mime-type from filename.
        /// </summary>
        /// <param name="content">File content</param>
        /// <param name="filename">File name, should include extension for mime mapping to work.</param>
        public FileResponse(Stream content, string filename)
            : this(content, filename, MimeMapping.GetMimeMapping(filename))
        {            
        }

        /// <summary>
        /// Initializes new FileResponse
        /// </summary>
        /// <param name="content">File content</param>
        /// <param name="filename">File name</param>
        /// <param name="contentType">Mime content type for file</param>
        public FileResponse(Stream content, string filename, string contentType)
        {
            Content = new StreamContent(content);

            Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };

            StatusCode = HttpStatusCode.OK;            
        }
    }
}

using System.Collections.Generic;
using System.Text;
using System.Web.Http.Controllers;

namespace Affecto.WebApi.Toolkit
{
    internal static class ActionContextExtensions
    {
        /// <returns>
        /// A string that represents the current parameters.
        /// </returns>
        public static string ParametersToString(this HttpActionContext actionContext)
        {
            StringBuilder builder = new StringBuilder();
            if (actionContext != null && actionContext.ActionArguments != null)
            {
                foreach (KeyValuePair<string, object> parameter in actionContext.ActionArguments)
                {
                    builder.Append(parameter.Key);
                    builder.Append(": ");
                    builder.Append(parameter.Value);
                    builder.Append(", ");
                }
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }
    }
}
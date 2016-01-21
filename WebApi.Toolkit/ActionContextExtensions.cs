using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Http.Controllers;

namespace Affecto.WebApi.Toolkit
{
    internal static class ActionContextExtensions
    {
        private const string ItemSeparator = ", ";

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
                    if (parameter.Value is ICollection)
                    {
                        builder.AppendLine();
                        builder.AppendLine("[");
                        foreach (var listItem in (ICollection)parameter.Value)
                        {
                            builder.Append("\t");
                            builder.AppendLine(listItem.ToString());
                        }
                        builder.Append("]");
                    }
                    else
                    {
                        builder.Append(parameter.Value);
                        builder.Append(ItemSeparator);
                    }
                }
            }
            string returnValue = builder.ToString();
            if (returnValue.EndsWith(ItemSeparator))
            {
                return returnValue.Remove(returnValue.Length - ItemSeparator.Length);
            }
            return returnValue;
        }
    }
}
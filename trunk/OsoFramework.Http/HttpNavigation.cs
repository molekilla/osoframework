// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OsoFramework.Http
{
    public class HttpNavigation : LinkedList<HttpNavigationStep>
    {
        private HttpCommand client = null;
        public HttpNavigation(HttpCommand command)
        {
            client = command;
        }
        internal string Read(HttpNavigationStep step)
        {
            return client.Read(step);
        }
        internal string Read(HttpNavigationStep step, params string[] parameters)
        {
            return client.Read(new HttpSettings
            {
                Query = String.Format(step.Query, parameters),
                ContentType = step.ContentType,
                UseNetworkCredentials = step.UseNetworkCredentials,
                UserAgent = step.UserAgent,
                Name = step.Name,
                ResponseEncoding = step.ResponseEncoding
            }
            );
        }
    }
}

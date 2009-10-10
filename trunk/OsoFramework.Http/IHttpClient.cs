// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OsoFramework.Http
{
    public interface IHttpClient
    {
        System.Net.CookieContainer CookieContainer {get;set;}
        bool SendServiceRequest(IHttpCommand httpCommand);
        void SendServiceRequestAsync(IHttpCommand httpCommand, object state);
    }
}

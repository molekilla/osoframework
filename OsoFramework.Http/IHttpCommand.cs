// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace OsoFramework.Http
{
    public interface IHttpCommand
    {
        bool KeepAlive { get; set; }
        string Referer { get; set; }
        string StatusDescription { get; set; }
        HttpStatusCode StatusCode { get; set; }
        Uri ResponseUri { get; set; }
        System.Net.WebHeaderCollection WebHeaders { get; set; }
        HttpSettings CurrentHttpSettings { get; set; }
        ValidateResultEventHandler ValidateMethod { get; set; }        
        CookieCollection Cookies { get; set; }
        byte[] StreamMessage { get; set; }
        bool DoWriteStream { get; set; }
        string Method { get; set; }
        NetworkCredential BasicAuthentication { get; set; }        
    }
}

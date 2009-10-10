// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OsoFramework.Http
{
    public class HttpSettings : ConfigurationElement
    {
        public HttpSettings() : base()
        {
            UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT)";
            ResponseEncoding = "ISO-8859-1";
            ContentType = "text/html";
            UseNetworkCredentials = true;
            Query = string.Empty;
            Name = "HttpSettings";
            
        }
        [ConfigurationProperty("name", IsKey = true, IsRequired=true, DefaultValue="ohla")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("userAgent")]
        public string UserAgent
        {
            get
            {
                return (string)this["userAgent"];
            }
            set
            {
                this["userAgent"] = value;
            }
        }
        [ConfigurationProperty("responseEncoding")]
        public string ResponseEncoding
        {
            get
            {
                return (string)this["responseEncoding"];
            }
            set
            {
                this["responseEncoding"] = value;
            }
        }
        [ConfigurationProperty("query")]
        public string Query
        {
            get
            {
                return (string)this["query"];
            }
            set
            {
                this["query"] = value;
            }
        }

        [ConfigurationProperty("contentType")]
        public string ContentType
        {
            get
            {
                return (string)this["contentType"];
            }
            set
            {
                this["contentType"] = value;
            }
        }
        [ConfigurationProperty("useNetworkCredetials")]
        public bool UseNetworkCredentials
        {
            get
            {
                return (bool)this["useNetworkCredetials"];
            }
            set
            {
                this["useNetworkCredetials"] = value;
            }
        }

        public HttpSettings AddQueryParameters(params string[] parameters)
        {
            this.Query = string.Format(this.Query, parameters);
            return this;
        }
    }
}

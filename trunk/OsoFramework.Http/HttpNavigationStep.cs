// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace OsoFramework.Http
{
	public class HttpNavigationStep : HttpSettings
	{

        public string ResponseData { get; set; }
        public HttpNavigationStep() : base()
        {
        }

        public T Parse<T>(Func<string, object> func) where T :  new()
        {
            return (T)func(ResponseData);
        }

        public IEnumerable<XElement> ParseXElement(Func<string, object> func)
        {
            return func(ResponseData) as IEnumerable<XElement>;
        }

        public void Parse(Action<string> func) 
        {
            func(ResponseData);
        }

	}
}

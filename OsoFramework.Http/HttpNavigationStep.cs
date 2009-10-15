// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;

namespace OsoFramework.Http
{
	public class HttpNavigationStep : HttpSettings
	{
        [ConfigurationProperty("nextStep")]
        public string NextStepName { get; set; }

        public string ResponseData { get; set; }

        IEnumerable<XElement> tempParseData;

        public IEnumerable<XElement> ParseResult
        {
            get { return tempParseData; }
            set { tempParseData = value; }
        }

        public HttpNavigationStep() : base()
        {
        }

        public T Parse<T>(Func<string, object> func) where T :  new()
        {
            return (T)func(ResponseData);
        }

        //public IEnumerable<XElement> ParseXElement(Func<string, object> func)
        //{
        //    return func(ResponseData) as IEnumerable<XElement>;
        //}

        public HttpNavigationStep ParseXElement(Func<string, object> func)
        {
            tempParseData =  func(ResponseData) as IEnumerable<XElement>;
            return this;
        }

        public void ReduceXElement(Action<XElement, int, int> func)
        {
            if (ParseResult != null)
            {
                int position = 1;
                int count = ParseResult.Count();
                foreach (var element in ParseResult)
                {
                    func(element, position, count);
                    position++;
                }
            }
        }

        public void Parse(Action<string> func) 
        {
            func(ResponseData);
        }

	}
}

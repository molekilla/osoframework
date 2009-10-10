// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace OsoFramework.Http
{
	[Obsolete]
    public class SecurityCommand : HttpCommand
    {

        public SecurityCommand()
        {
            CreateDefaultHttpClient();
        }

        /// <summary>
        /// Parses the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private XDocument Parse(string response)
        {
            XDocument xmlDocument = new XDocument();
            string cleanResponse = response.Replace("<html  xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">", "<html>");
            bool isOk = this.TryParseToXml(cleanResponse, out xmlDocument);

            if (isOk)
            {
                return xmlDocument;
            }
            else
            {
                return null;
            }
        }


        public string[] GetAllElementsHtmlByName(HttpSettings settings, string name)
        {
            XDocument document = null;
            List<string> elements = new List<string>();

            this.Get(settings,
                (response, sender) =>
                {
                    document = Parse(response);

                    if (document != null)
                    {
                        List<XElement> list = document.Descendants(name).ToList();
                        foreach (XElement el in list)
                        {
                            elements.Add(el.ToString());
                        }
                    }
                }).Execute();

            return elements.ToArray();
        }


        public string[] GetAllElementsValueByName(HttpSettings settings,  string name)
        {
            XDocument document = null;
            List<string> elements = new List<string>();

            this.Get(settings,
                (response, sender) =>
                {
                    document = Parse(response);

                    if (document != null)
                    {
                        List<XElement> list = document.Descendants(name).ToList();
                        foreach (XElement el in list)
                        {
                            elements.Add(el.Value);
                        }
                    }
                }).Execute();

            return elements.ToArray();
        }

        public string[] ExecuteLinkValueTest(HttpSettings settings,string getParameterTest )
        {
            XDocument document = null;


            List<string> linksToTest = new List<string>();
            this.Get(settings,
                (response, command) =>
                {
                    document = Parse(response);

                    if (document != null)
                    {
                        List<XElement> list = document.Descendants("A").ToList();
                        foreach (XElement el in list)
                        {
                            string url = el.Attribute("HREF").Value;
                            Uri ahref = null;
                            if (url.ToLower().StartsWith("http:"))
                            {
                                ahref = new Uri(url);
                            }
                            else
                            {
                                ahref = new Uri(new Uri(command.CurrentHttpSettings.Query), url);
                            }

                            string newQuery = UriParser.FillParameters(ahref.ToString(), "?", "&", getParameterTest);
                            linksToTest.Add(newQuery);
                        }
                    }

                }).Execute();


            return linksToTest.ToArray();
        }

        public void ExecutePostValueTest(HttpSettings settings, string elementName, string postParameterTest)
        {
        }


        public XDocument GetDocumentForUrlPS(HttpSettings settings,  PreparserDelegate preparserHandler)
        {
            XDocument document = null;

            this.Get(settings, (response, sender) =>
            {
                string result = preparserHandler(response);

                if (!TryParseToXml(result, out document))
                {
                    document = null;
                }
            }).Execute();

            return document;
        }

        public XDocument PostDocumentForUrlPS(HttpSettings settings, string data, PreparserDelegate preparserHandler)
        {
            XDocument document = null;

            this.Post(settings, data, (response, sender) =>
            {
                string result = preparserHandler(response);

                if (!TryParseToXml(result, out document))
                {
                    document = null;
                }
            }).Execute();

            return document;
        }

        public delegate string PreparserDelegate(string response);
    }
}

// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Sgml;
using System.Xml;
using System.Xml.Linq;

namespace OsoFramework.Http
{
    public partial class HttpCommand
    {

        public XDocument GetTagElements(string data, string startTag, string endTag)
        {
            string tagg = startTag;
            int start = data.IndexOf(tagg);
            if (!tagg.EndsWith(">"))
            {
                start = data.IndexOf(">", start);
            }
            int end = 0;
            try
            {
                end = data.IndexOf(endTag, start);
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
            StringBuilder buffer = new StringBuilder();

            do
            {
                buffer.Append(data.Substring(start, (end - start) + endTag.Length));
                if (end > -1)
                {
                    start = data.IndexOf(tagg, end);
                }
                if (start > -1)
                {
                    end = data.IndexOf(endTag, start);
                }

            } while (start > -1 && end > -1);

            string body = buffer.ToString();
            string html = ("<html>" + body + "</html>").Replace("<html>>", "<html>");
            XDocument document = null;
            TryParseToXml(html, out document);

            return document;

        }

    }
}

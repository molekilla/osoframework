// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OsoFramework.Http
{
    public static class  HttpXmlDocumentExtensions
    {
        public static bool  HasAttributeValue(this XElement element,string id, string value)
        {
            return element.Attribute(id) != null && element.Attribute(id).Value == value;
        }

        public static IEnumerable<XElement> HtmlBreaks(this XContainer parent)
        {
            return parent.Descendants(HTML.BR);
        }


        public static IEnumerable<XElement> HtmlOptions(this XContainer parent)
        {
            return parent.Descendants(HTML.OPTION);
        }


        public static IEnumerable<XElement> HtmlSelects(this XContainer parent)
        {
            return parent.Descendants(HTML.SELECT);
        }


        public static IEnumerable<XElement> HtmlInputs(this XContainer parent)
        {
            return parent.Descendants(HTML.INPUT);
        }


        public static IEnumerable<XElement> HtmlForms(this XContainer parent)
        {
            return parent.Descendants(HTML.FORM);
        }


        public static IEnumerable<XElement> HtmlParagraphs(this XContainer parent)
        {
            return parent.Descendants(HTML.P);
        }

        public static IEnumerable<XElement> HtmlBoldItems(this XContainer parent)
        {
            return parent.Descendants(HTML.B);
        }


        public static IEnumerable<XElement> HtmlImages(this XContainer parent)
        {
            return parent.Descendants(HTML.IMG);
        }

        public static IEnumerable<XElement> HtmlSpans(this XContainer parent)
        {
            return parent.Descendants(HTML.SPAN);
        }

        public static IEnumerable<XElement> HtmlAnchors(this XContainer parent)
        {
            return parent.Descendants(HTML.A);
        }


        public static IEnumerable<XElement> HtmlPreformattedItems(this XContainer parent)
        {
            return parent.Descendants(HTML.PRE);
        }


        public static IEnumerable<XElement> HtmlTableHeaders(this XContainer parent)
        {
            return parent.Descendants(HTML.THEAD);
        }


        public static IEnumerable<XElement> HtmlTableBodies(this XContainer parent)
        {
            return parent.Descendants(HTML.TBODY);
        }


        public static IEnumerable<XElement> HtmlTables(this XContainer parent)
        {
            return parent.Descendants(HTML.TABLE);
        }

        public static IEnumerable<XElement> HtmlTableRows(this XContainer parent)       
        {
            return parent.Descendants(HTML.TR);
        }

        public static IEnumerable<XElement> HtmlTableCells(this XContainer parent)
        {
            return parent.Descendants(HTML.TD);
        }

    }
}

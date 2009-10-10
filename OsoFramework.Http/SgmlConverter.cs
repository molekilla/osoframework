// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Sgml;

namespace OsoFramework.Http
{
    public class SgmlConverter
    {
        public static string ParseHtml(string html)
        {
            try
            {
                SgmlReader reader = new SgmlReader();
                reader.DocType = "HTML";
                reader.InputStream = new StringReader(html);
                reader.CaseFolding = CaseFolding.ToUpper;
                
                StringWriter sw = new StringWriter();
                XmlTextWriter w = new XmlTextWriter(sw);
                w.Formatting = Formatting.Indented;
                reader.WhitespaceHandling = WhitespaceHandling.None;
                while (!reader.EOF)
                {
                    w.WriteNode(reader, true);
                }
                w.Close();
                return sw.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return string.Empty;
            }
        }
    }
}

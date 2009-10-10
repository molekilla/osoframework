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

    public static class HttpFormExtensions
    {
        public static IEnumerable<XElement> DescendantsFromAttributeWithValue(this XDocument document, string elementName, string attributeName, string attributeValue)
        {
            return from el in document.Descendants(elementName)
                   where el.Attribute(attributeName) != null 
                   && el.Attribute(attributeName).Value == attributeValue
                   select el;
        }
        /// <summary>
        /// Creates the query string from form fields.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="append">The append.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string CreateQueryStringFromFormFields(this XDocument document, string append, params string[] values)
        {
            var form = HttpFormExtensions.GetFormFields(document);
            StringBuilder buffer = new StringBuilder();

            if (values.Length == 1 && values[0].Length == 0)
            {
                foreach (KeyValuePair<string, List<string>> pair in form)
                {
                    buffer.Append(System.Web.HttpUtility.UrlEncode(pair.Key) + "=" + System.Web.HttpUtility.UrlEncode(pair.Value[0]));
                    buffer.Append("&");
                }
            }
            else
            {
                foreach (string pair in values)
                {
                    string key = pair.Split(':')[0];
                    int index = int.Parse(pair.Split(':')[1]);
                    buffer.Append(System.Web.HttpUtility.UrlEncode(key) + "=" + System.Web.HttpUtility.UrlEncode(form[key][index]));
                    buffer.Append("&");
                }
            }
            buffer.Append(append);

            return buffer.ToString();
        }

        /// <summary>
        /// Gets the form fields.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetFormFields(this XDocument document)
        {
            var data = (from input in document.Descendants("INPUT")
                        where (input.Attribute("NAME") != null && input.Attribute("NAME").Value != null)
                        select input);
            var selects = (from sel in document.Descendants("SELECT")
                           where (sel.Attribute("NAME") != null && sel.Attribute("NAME").Value != null)
                           select sel);

            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();

            foreach (XElement el in data)
            {
                if (!values.ContainsKey(el.Attribute("NAME").Value))
                {
                    values.Add(el.Attribute("NAME").Value, new List<string>());
                }
                if (el.Attribute("VALUE") == null)
                {
                    values[el.Attribute("NAME").Value].Add(string.Empty);
                }
                else
                {
                    values[el.Attribute("NAME").Value].Add(el.Attribute("VALUE").Value);
                }
            }

            foreach (XElement selectElement in selects)
            {
                if (!values.ContainsKey(selectElement.Attribute("NAME").Value))
                {
                    values.Add(selectElement.Attribute("NAME").Value, new List<string>());
                }

                XElement[] optionSelected = (from option in selectElement.Descendants("OPTION")
                                             where (option.Attribute("SELECTED") != null && option.Attribute("SELECTED").Value == "selected")
                                             select option).ToArray();
                if (optionSelected.Length > 0)
                {
                    values[selectElement.Attribute("NAME").Value].Add(optionSelected[0].Attribute("VALUE").Value);
                }
                else
                {
                    var optionNoSelected = (from option in selectElement.Descendants("OPTION")
                                            where (option.Attribute("SELECTED") != null && option.Attribute("SELECTED").Value == "")
                                            select option).ToArray();

                    if (optionNoSelected.Length > 0)
                    {
                        values[selectElement.Attribute("NAME").Value].Add(optionNoSelected[0].Attribute("VALUE").Value);
                    }
                    else
                    {
                        string value = (from option in selectElement.Descendants("OPTION")
                                        select option).ToArray()[0].Attribute("VALUE").Value;

                        values[selectElement.Attribute("NAME").Value].Add(value);
                    }
                }
            }

            return values;
        }

        public static Dictionary<string, List<string>> GetSelectFields(this XDocument document)
        {
            var selects = (from sel in document.Descendants("SELECT")
                           where (sel.Attribute("NAME") != null && sel.Attribute("NAME").Value != null)
                           select sel);

            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();

            foreach (XElement selectElement in selects)
            {
                if (!values.ContainsKey(selectElement.Attribute("NAME").Value))
                {
                    values.Add(selectElement.Attribute("NAME").Value, new List<string>());
                }

                XElement[] options = (from option in selectElement.Descendants("OPTION")
                                             select option).ToArray();
                if (options.Length > 0)
                {
                    foreach (var opt in options)
                    {
                        if (opt.Attribute("VALUE") != null)
                        {
                            values[selectElement.Attribute("NAME").Value].Add(opt.Attribute("VALUE").Value);
                        }
                    }
                }
            }

            return values;
        }

    }
}

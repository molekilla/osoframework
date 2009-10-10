// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace OsoFramework.Http
{
	/// <summary>
	/// Summary description for UriParser.
	/// </summary>
	public class UriParser
	{
		public UriParser()
		{
		}

        public static string FillParameters(string data, string separator, string nameValueSeparator, string value)
        {
            Dictionary<string, List<string>> parameters = ConvertQueryFrom(data, separator, nameValueSeparator);
            return ConvertQueryTo(parameters, separator, nameValueSeparator, value);
        }

		/// <summary>
		/// Converts the post data string to a Hashtable.
		/// </summary>
		/// <param name="data"> The post data string.</param>
		/// <param name="separator"> The main separator for the string.</param>
		/// <param name="nameValueSeparator"> The name value pair separator.</param>		
        public static Dictionary<string, List<string>> ConvertQueryFrom(string data, string separator, string nameValueSeparator)
		{
			// PostData
            Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>>();
			string[] nameValueArray = data.Split(separator.ToCharArray());

            if (nameValueArray.Length == 2)
            {
                string[] queryParameters = nameValueArray[1].Split(nameValueSeparator.ToCharArray());

                foreach (string parameter in queryParameters)
                {
                    string[] pair = parameter.Split('=');
                    
                        string name = EncodeDecode.UrlDecode(pair[0]);
                        if ( parameters.ContainsKey(name) )
                        {
                            parameters.Add(name, new List<string>());
                        }
                        parameters[name].Add(pair[1]);
                    
                }
            }
			

			return parameters;
		}


        /// <summary>
        /// Converts the query string to an string.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        /// <param name="nameValueSeparator"></param>
        /// <returns></returns>
        public static string ConvertQueryTo(Dictionary<string, List<string>> data, string separator, string nameValueSeparator, string testValue)
        {
            // QueryString
            StringBuilder queryString = new StringBuilder();

            foreach (KeyValuePair<string, List<string>> de in data)
            {                     
                if (nameValueSeparator.Length == 0)
                {
                    queryString.Append(separator);
                    queryString.Append(testValue);
                }
                else
                {
                    foreach (string s in de.Value)
                    {
                        queryString.Append(de.Key);
                        queryString.Append(nameValueSeparator);
                        queryString.Append(testValue);
                        queryString.Append(separator);
                    }
                }
            }

            return queryString.ToString();
        }

	}
}

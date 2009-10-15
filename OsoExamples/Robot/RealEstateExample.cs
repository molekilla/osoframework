// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using OsoFramework;
using OsoFramework.Http;
using SubSonic;
using OsoExamples.DatabaseSchema;

namespace OsoExamples.Robot
{
    public class RealEstateExample : WebRobotBase, IWebRobot
    {
        System.IFormatProvider SpanishDateFormat = new System.Globalization.CultureInfo("es-ES", true);

        public void Start()
        {
            Main();
        }

        private void Main()
        {
            // using read
            string goog1 = Read(new HttpSettings { Query = "http://www.google.com" });

            // using get XDocument for url
            // pre parsing is for manually parsing non conforming XHTML
            var goog2 = ReadXml(new HttpSettings { Query = "http://www.google.com" }, x => x);

            byte[] a = ReadWebBinaryResource("http://subsonicproject.com/content/images/SubSonicSMall.png");
            
            // Example: Parse site using registed navigation steps
            // Get states
            // Navigation
            //  
            Navigation
                .FindByName("start")
                .Read()
                .ParseXElement
                (data =>
                    {
                        return from lnk in GetTagElements(data, "<body", "</body>").HtmlAnchors()
                                where lnk.Attribute("HREF") != null
                                    && lnk.Attribute("HREF").Value.StartsWith("/sv/buscar.html")
                                select lnk;
                    }
                ).ReduceXElement(ReadState);

            // reduce
        }

        private void ReadState(XElement item, int position, int count)
        {
            string state = item.Value;
            Print(String.Format("Downloading {0} / {1} from site with state {2}",
                GetPercentageFrom(position, count), "100%", state));

            // Step 2: Provincia
            Navigation
                .FindByName("provincia")
                .Read(item.Attribute("HREF").Value)
                .ParseXElement
                (data =>
                    {
                        return (from div in GetTagElements(data, "<body", "</body>").Descendants("DIV")
                                where div.Attribute("CLASS") != null
                                && div.Attribute("CLASS").Value == "caja_ficha_370"
                                && div.Descendants("H3").Count() == 1
                                select div).FirstOrDefault().HtmlAnchors();
                    }
                ).ReduceXElement(ReadPlace);

            // Reduce
   
        }

        private void ReadPlace(XElement item, int position, int count)
        {
                int start = item.Value.IndexOf("(");
                string area = item.Value.Substring(0, start);

                // Find  if item exists
                var existingItem = DatabaseRepository.FindBy<RealEstateAd>(area);
                if (existingItem != null && existingItem.Count() == 0)
                {
                    DatabaseRepository.Insert<RealEstateAd>(
                        new RealEstateAd
                        {
                            Provincia = area
                        });
                    Print(area + " added.");
                }
                else
                {
                    var editItem = existingItem.FirstOrDefault();
                    editItem.LastUpdated = DateTime.Now;
                    editItem.Provincia = area;
                    DatabaseRepository.Update<RealEstateAd>(editItem);
                    Print(area + " updated.");
                }
            }
        }

    }


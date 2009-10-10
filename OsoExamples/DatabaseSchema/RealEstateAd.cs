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
using OsoFramework;

namespace OsoExamples.DatabaseSchema
{


	public class RealEstateAd : IParseData 
	{

		public RealEstateAd ()
		{
            Provincia = string.Empty;
            LastUpdated = DateTime.Now;
		}
		
        /// <summary>
        /// by default we have a autonumber
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// KeyIndex is the column we want to index, useful for querying existing items.
        /// </summary>
        public string KeyIndex
        {
            get
            {
                return Provincia;
            }
            set
            {
                Provincia = value;
            }
        }

        public string Provincia { get; set; }
        public DateTime LastUpdated { get; set; }
	}
}

// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using OsoFramework.Http;
using System.Configuration;

namespace OsoFramework
{
	public class NavigationConfiguration : ConfigurationSection
	{
        public NavigationConfiguration()
        {
        }

        [ConfigurationProperty("steps", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NavigationStepConfigCollection),
            AddItemName = "add")]
        public NavigationStepConfigCollection NavigationSteps
        {
            get
            {
                NavigationStepConfigCollection items =
                    (NavigationStepConfigCollection)base["steps"];
                return items;
            }
        }

	}
}

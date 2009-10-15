using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace OsoFramework
{
	internal class ServiceLocator
	{
        public static IEnumerable<IWebRobot> ReadWebRobotsFromConfiguration()
        {
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(container);


            IEnumerable<IWebRobot> robots = container.ResolveAll<IWebRobot>();
            return robots;
        }
	}
}

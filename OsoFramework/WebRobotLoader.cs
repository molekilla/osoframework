// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.Configuration;
using log4net;
using log4net.Config;

namespace OsoFramework
{
    public class WebRobotLoader
    {
		private static ILog Log = LogManager.GetLogger(typeof(WebRobotLoader));
        
		public WebRobotLoader()
        {
        }

        public static void Run()
        {
			
            try
            {
				XmlConfigurator.Configure();
			
                IUnityContainer container = new UnityContainer();
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                section.Containers.Default.Configure(container);


                IEnumerable<IWebRobot> robots = container.ResolveAll<IWebRobot>();

                foreach (IWebRobot robot in robots)
                {
                    // You can also use BuildUp to set DataWriter type
                    robot.InitializeHttpCommand();
                    StartRobot(robot);
                }
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.ToString());
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				Log.Error(ex);
          
            }
        }
		
		private static void StartRobot(IWebRobot robot)
		{
			int retries = 0;
   			do
            {
                try
                {
                    robot.Start();
                }
                catch (System.Net.WebException webex)
                {
                    Log.Debug("OsoFx Ignored Error", webex);
                    // ignore
                }
                catch (Exception ex)
                {
                    Log.Error("Oso Fx: " + robot.Name + " robot", ex);
                    retries++;
                }
            }
            while (retries < 25);			
		}
    }
}

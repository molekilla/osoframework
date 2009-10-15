using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using OsoFramework.Management;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Web;

namespace OsoFramework
{
	public class WebRobotLogServiceLoader
	{
        static WebServiceHost host;

  

        public static void OpenService(Type serviceType)
        {
            host = new WebServiceHost(serviceType,
            new Uri(ConfigurationManager.AppSettings["OsoFx.LogServiceUrl"]));
            
            try
            {
                // Start WCF REST Service
                ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IWebManagementLog), new WebHttpBinding(), "");
                host.Description.Behaviors.Find<ServiceDebugBehavior>().HttpHelpPageEnabled = false;
                host.Open();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
	}
}

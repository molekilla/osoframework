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
using System.ServiceModel.Web;
using OsoFramework.Management;
using System.ServiceModel.Description;
using System.ServiceModel;
using OsoFrameworkManager;

namespace OsoFramework
{
    public class WebRobotLoader
    {
        private static ILog Log = LogManager.GetLogger(typeof(WebRobotLoader));

        static WebServiceHost host;

        static WebRobotLoader()
        {
            XmlConfigurator.Configure();
            host = new WebServiceHost(typeof(WebRobotDashboard),
            new Uri(ConfigurationManager.AppSettings["OsoFx.ServiceUrl"]));
        }

        private static void PingService()
        {
            ChannelFactory<IWebRobotService> cf = null;
            try
            {
                cf = new ChannelFactory<IWebRobotService>(new WebHttpBinding(), ConfigurationManager.AppSettings["OsoFx.ServiceUrl"]);
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                var a = channel.ListWebRobots();
                cf.Close();
            }
            catch (Exception ex)
            {
                cf.Abort();
                Console.WriteLine(ex);
                // ignore, probably client down
            }
        }
        public static void RunService()
        {

            try
            {
                // Start WCF REST Service
                ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IWebRobotService), new WebHttpBinding(), "");
                host.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
                host.Description.Behaviors.Find<ServiceDebugBehavior>().HttpHelpPageEnabled = false;

                host.Open();
                PingService();
                Log.Info("OsoFx Service is running at " + ConfigurationManager.AppSettings["OsoFx.ServiceUrl"]);
                Console.WriteLine("OsoFx Service is running at {0}", ConfigurationManager.AppSettings["OsoFx.ServiceUrl"]);
                Console.WriteLine("Press ANY KEY to exit");
                Console.Read();
                host.Close();

                LogServiceClient logclient = new LogServiceClient();
                logclient.WriteLog(new WebRobotStreamLogLine[] 
                {
                    new WebRobotStreamLogLine 
                    {
                        Line = "service closed",
                        RobotName = "all" , 
                        Timestamp = DateTime.Now 
                    }
                });
                logclient.PingStatus("all", WebRobotManagementStatus.STOPPED);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Log.Error(ex);
            }
        }
        public static void RunDirect()
        {

            try
            {
                var robots = ServiceLocator.ReadWebRobotsFromConfiguration();

                foreach (IWebRobot robot in robots)
                {
                    WebRobotProcess.Run(new WebRobotProcess { WebRobot = robot });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Log.Error(ex);
            }
        }


    }
}

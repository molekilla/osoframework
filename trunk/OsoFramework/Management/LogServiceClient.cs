using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OsoFramework.Management;
using System.ServiceModel.Description;
using OsoFrameworkManager;
using OsoFramework;
using System.Configuration;
using System.Threading;

namespace OsoFrameworkManager
{
    public class LogServiceClient
    {
        static object locked = "locked";
        
        static string url;
        static LogServiceClient()
        {
            url = ConfigurationManager.AppSettings["OsoFx.LogServiceUrl"];
        }

        public void PingStatus(string name, WebRobotManagementStatus status)
        {
            Action<string, WebRobotManagementStatus> runAction = (x, y) =>
                {
                    if (status == WebRobotManagementStatus.STOPPED ||
                        status == WebRobotManagementStatus.ERROR)
                    {
                        ChannelFactory<IWebManagementLog> cf = null;
                        try
                        {
                            cf = new ChannelFactory<IWebManagementLog>(new WebHttpBinding(), url);
                            cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                            var channel = cf.CreateChannel();

                            channel.PingRobotStatus(x, y);
                            cf.Close();
                        }
                        catch (Exception ex)
                        {
                            cf.Abort();
                            Console.WriteLine(ex);
                            // ignore, probably client down
                        }
                    }
                };
            runAction.BeginInvoke(name,status, r => { runAction.EndInvoke(r); }, null);
        }

        public  void WriteLog(WebRobotStreamLogLine[] lines)
        {
            Action<WebRobotStreamLogLine[]> runAction = x =>
            {
                ChannelFactory<IWebManagementLog> cf = null;
                try
                {
                    cf = new ChannelFactory<IWebManagementLog>(new WebHttpBinding(), url);
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    var channel = cf.CreateChannel();


                    channel.WriteLog(new WebRobotStreamLog
                           {
                               Lines = x
                           });

                    cf.Close();
                }
                catch (Exception ex)
                {
                    cf.Abort();
                    Console.WriteLine(ex);
                    // ignore, probably client down
                }
            };
            runAction.BeginInvoke(lines, r=> { runAction.EndInvoke(r); }, null );
            
        }

    }
}

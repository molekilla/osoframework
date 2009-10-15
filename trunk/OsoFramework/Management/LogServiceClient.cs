using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OsoFramework.Management;
using System.ServiceModel.Description;
using OsoFrameworkManager;
using OsoFramework;

namespace OsoFrameworkManager
{
    public class LogServiceClient
    {
        int limit = 10;
        List<WebRobotStreamLogLine> logBuffer = new List<WebRobotStreamLogLine>();
        string url;
        public LogServiceClient(string endpointUrl)
        {
            url = endpointUrl;
        }

        public void PingStatus(string name, WebRobotManagementStatus status)
        {
            ChannelFactory<IWebManagementLog> cf = null;
            try
            {
                cf = new ChannelFactory<IWebManagementLog>(new WebHttpBinding(), url);
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();

                channel.PingRobotStatus(name, status);
                cf.Close();
            }
            catch (Exception ex)
            {
                cf.Abort();
                Console.WriteLine(ex);
                // ignore, probably client down
            }

        }

        public void WriteLog(WebRobotStreamLogLine[] lines)
        {
            ChannelFactory<IWebManagementLog> cf = null;
            try
            {
                cf = new ChannelFactory<IWebManagementLog>(new WebHttpBinding(), url);
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();

                if (logBuffer.Count >= limit)
                {
                    channel.WriteLog(new WebRobotStreamLog
                           {
                               Lines = logBuffer.ToArray()
                           });

                    logBuffer.Clear();
                }
                else
                {
                    logBuffer.AddRange(lines);
                }
                cf.Close();
            }
            catch (Exception ex)
            {
                cf.Abort();
                Console.WriteLine(ex);
                // ignore, probably client down
            }

        }

    }
}

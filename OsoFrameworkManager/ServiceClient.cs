using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OsoFramework.Management;
using System.ServiceModel.Description;
using OsoFrameworkManager;

namespace OsoFrameworkManager
{
    public class ServiceClient
    {
        string url;
        public ServiceClient(string endpointUrl)
        {
            url = endpointUrl;
        }
        public void Stop(string name)
        {
            using (ChannelFactory<IWebRobotService> cf =
               new ChannelFactory<IWebRobotService>(new WebHttpBinding(), url))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                channel.Stop(name);
            }

        }

        public void Start(string name)
        {
            using (ChannelFactory<IWebRobotService> cf =
               new ChannelFactory<IWebRobotService>(new WebHttpBinding(), url))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                channel.Start(name);
            }

        }
        public void AddRobotScript(string name, string code)
        {
            using (ChannelFactory<IWebRobotService> cf =
               new ChannelFactory<IWebRobotService>(new WebHttpBinding(), url))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var channel = cf.CreateChannel();
                channel.AddRobotScript(name,code);
            }
           
        }

        public  WebRobotItemCollection GetRobotItems()
        {
            WebRobotItemCollection displayItems = new WebRobotItemCollection();

            try
            {

                using (ChannelFactory<IWebRobotService> cf =
                    new ChannelFactory<IWebRobotService>(new WebHttpBinding(), url))
                {
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    var channel = cf.CreateChannel();
                    var items = channel.ListWebRobots();

                    foreach (var item in items)
                    {
                        displayItems.Add(WebRobotItem.Convert(item));
                    }
                }

            }
            catch
            {
                // error
            }
            return displayItems;

        }
    }
}

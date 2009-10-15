using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Threading;

namespace OsoFramework
{
    /// <summary>
    /// Thread safe collection
    /// Handles the instantiation of robots
    /// </summary>
    public class WebRobotManager : SynchronizedKeyedCollection<string, WebRobotProcess>
    {
        public WebRobotManager()
            : base()
        {
        }

        public void Start(string name)
        {
            var robot = this[name];
            if (robot != null)
                WebRobotProcess.Run(robot);
        }



        public IWebRobot[] GetWebRobots()
        {
            List<IWebRobot> robots = new List<IWebRobot>();
            foreach (var process in this)
            {
                robots.Add(process.WebRobot);
            }
            return robots.ToArray();
        }
        public void Add(IWebRobot robot)
        {
            this.Add(new WebRobotProcess { WebRobot = robot });
        }
        protected override string GetKeyForItem(WebRobotProcess item)
        {
            return item.WebRobot.Name;
        }
    }
}

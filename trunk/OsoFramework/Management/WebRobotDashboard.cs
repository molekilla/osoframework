using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace OsoFramework.Management
{
    public class WebRobotDashboard : IWebRobotService
	{
        static WebRobotManager robotManager;
        static WebRobotDashboard()
        {
            Initialize();
        }
        static void Initialize()
        {
            try
            {
                // Add robots to WebRobotManager
                robotManager = new WebRobotManager();
                var robots = ServiceLocator.ReadWebRobotsFromConfiguration();

                foreach (IWebRobot robot in robots)
                {
                    robotManager.Add(robot);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Console.WriteLine(ex.ToString());
            }
        }
      
        public WebRobotDashboardItem[] ListWebRobots()
        {
            List<WebRobotDashboardItem> items = new List<WebRobotDashboardItem>();
            foreach (var item in robotManager.GetWebRobots())
            {
                items.Add(new WebRobotDashboardItem
                {
                    DatabaseConnectionString = item.DatabaseRepository.ConnectionString,
                    Name = item.Name,
                    ScriptingCode = string.Empty,
                    Status = item.Status
                });
            }
            return items.ToArray();
        }

        public DashboardResponse Stop(string name)
        {
            robotManager[name].Stop();
            
            return new DashboardResponse { Message = "Stopped", Passed = true };
        }

        public DashboardResponse Start(string name)
        {
            robotManager[name].Run();

            return new DashboardResponse { Message = "Started", Passed = true };
        }

        public WebRobotDashboardItem GetStatus(string name)
        {
            var robot = robotManager[name].WebRobot;
            return new WebRobotDashboardItem
            {
                Name = name,
                DatabaseConnectionString = robot.DatabaseRepository.ConnectionString,
                ScriptingCode = string.Empty,
                Status = robot.Status
            };
        }

    }
}

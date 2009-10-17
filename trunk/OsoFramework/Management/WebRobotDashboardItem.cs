using System;
using System.Collections.Generic;
using System.Text;

namespace OsoFramework.Management
{
	public class WebRobotDashboardItem
	{
        public WebRobotDashboardItem()
        {
        }

        public string Name { get; set; }
        public WebRobotManagementStatus Status { get; set; }
        public string DatabaseConnectionString { get; set; }
	}
}

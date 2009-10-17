using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OsoFramework.Management;
using System.Windows.Threading;
using OsoFramework;

namespace OsoFrameworkManager.Management
{
    public class WebManagementLogService : IWebManagementLog
    {
        public static event Action<string> OnWriteLogEvent;
        public static event Action<string, WebRobotManagementStatus> OnWebRobotStatus;

        public void PingRobotStatus(string name, WebRobotManagementStatus state)
        {
            if (OnWebRobotStatus != null)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(OnWebRobotStatus, name, state);
            }
        }
        public void WriteLog(WebRobotStreamLog log)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (var line in log.Lines)
            {
                buffer.AppendFormat("[{0}] [{2}] - {1}",
                    line.Timestamp.ToString(), line.Line, line.RobotName);
                buffer.AppendLine();
            }

            if (OnWriteLogEvent != null)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(OnWriteLogEvent, buffer.ToString());
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OsoFrameworkManager
{
    public static class AppCommands
    {
        public static readonly RoutedUICommand StartStopRobotCommand;
       
        static AppCommands()
        {
            StartStopRobotCommand = new RoutedUICommand("Start/Stop Robot", "StartStopRobotCommand", typeof(AppCommands));
            //StopRobotCommand = new RoutedUICommand("Stop Robot", "StopRobotCommand", typeof(AppCommands));
        }
    }
}

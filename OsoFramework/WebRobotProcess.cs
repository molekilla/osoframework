using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;

namespace OsoFramework
{
    public class WebRobotProcess
	{
        Thread thread = new Thread(ProcessRobot);
        private static ILog Log = LogManager.GetLogger(typeof(WebRobotProcess));

        public WebRobotProcess() 
        {
            thread.IsBackground = true;
            WebRobot = null;
        }

        
        public IWebRobot WebRobot { get; set; }

        private static void ProcessRobot(object state)
        {
            var process = state as WebRobotProcess;
            var robot = process.WebRobot;

            try
            {
                robot.InitializeHttpCommand();
                robot.Status = WebRobotManagementStatus.RUNNING;
                robot.Start();
            }
            catch (ThreadAbortException abort)
            {
                robot.Status = WebRobotManagementStatus.STOPPED;
            }
            catch (Exception ex)
            {
                robot.Status = WebRobotManagementStatus.ERROR;
                Log.Error("Oso Fx: " + robot.Name + " robot", ex);
            }
        }


        public void Stop()
        {
            Log.Info(this.WebRobot.Name + " stopped.");
            this.thread.Abort();
            thread = new Thread(ProcessRobot);
        }
        
        public static void Run(WebRobotProcess process)
        {

            process.Run();
        }

        public void Run()
        {
            if (this.WebRobot.Status != WebRobotManagementStatus.RUNNING)
            {
                try
                {
                    Log.Info(this.WebRobot.Name + " running.");
                    thread.Start(this);
                }
                catch (Exception ex)
                {
                    lock (WebRobot)
                    {
                        this.WebRobot.Status = WebRobotManagementStatus.ERROR;
                    }
                    Log.Error("Oso Fx: " + this.WebRobot.Name + " robot", ex);
                }
            }
        }
	}
}

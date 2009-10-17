using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;
using OsoFrameworkManager;
using OsoFramework.Management;

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
            
                // ended
                Thread.CurrentThread.Abort();
               
            }
            catch (ThreadAbortException abort)
            {
                robot.Status = WebRobotManagementStatus.STOPPED;
            }
            catch (Exception ex)
            {
                robot.Status = WebRobotManagementStatus.ERROR;
                Log.Error("Oso Fx: " + robot.Name + " robot", ex);
                LogServiceClient log = new LogServiceClient();
                log.WriteLog(new WebRobotStreamLogLine[]
                {
                    new WebRobotStreamLogLine
                    {
                         Line = ex.ToString(),
                         RobotName = robot.Name,
                         Timestamp = DateTime.Now
                    }
                });
            }
        }


        public void Stop()
        {
            Log.Info(this.WebRobot.Name + " stopped.");
            this.thread.Abort();
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
                    if (!thread.IsAlive)
                    {
                        if (thread.ThreadState == ThreadState.Stopped
                            || thread.ThreadState == ThreadState.Aborted)
                        {
                            thread = new Thread(ProcessRobot);
                        }
                        if (thread.Name == null || thread.Name.Length == 0)
                        {
                            thread.Name = this.WebRobot.Name;
                        }
                        Log.Info(this.WebRobot.Name + " running.");
                        thread.Start(this);
                    }
                }
                catch (Exception ex)
                {     
                    Log.Error("Oso Fx: " + this.WebRobot.Name + " robot", ex);
                }
            }
        }
	}
}

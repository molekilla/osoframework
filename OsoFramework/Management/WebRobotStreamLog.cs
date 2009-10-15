using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OsoFramework.Management
{
	public class WebRobotStreamLog
	{
        
        public WebRobotStreamLog()
        {
            Lines = new WebRobotStreamLogLine[] { };
        }
 
        public WebRobotStreamLogLine[] Lines
        {
            get;
            set;
        }
	}
}

// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OsoFramework
{
    public interface IWebRobot
    {
        WebRobotManagementStatus Status
        {
            get;
            set;
        }
		string Name
		{
			get;
			
		}
		void InitializeHttpCommand();
        
		IDataRepository DatabaseRepository 
		{
			get;
			set;
		}
        
		void Start();
    }
}

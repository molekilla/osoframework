using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace OsoFramework.Management
{
    [ServiceContract]
	public interface IWebManagementLog
	{
        [OperationContract()]
        [WebInvoke(UriTemplate = "WriteLog", Method = "POST")]
        void WriteLog(WebRobotStreamLog log);

        [OperationContract()]
        [WebInvoke(UriTemplate = "PingRobotStatus", Method = "POST", BodyStyle=WebMessageBodyStyle.Wrapped)]
        void PingRobotStatus(string name, WebRobotManagementStatus status);
	}
}

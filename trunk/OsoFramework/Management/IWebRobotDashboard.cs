using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace OsoFramework.Management
{

    [ServiceContract]
	public interface IWebRobotService
	{
        [OperationContract]
        [WebInvoke(UriTemplate = "ListWebRobots", Method = "GET",BodyStyle=WebMessageBodyStyle.Wrapped)]
        WebRobotDashboardItem[] ListWebRobots();

        [OperationContract]
        [WebInvoke(UriTemplate = "Stop", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        DashboardResponse Stop(string name);

        [OperationContract]
        [WebInvoke(UriTemplate = "Start", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        DashboardResponse Start(string name);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetStatus", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        WebRobotDashboardItem GetStatus(string name);
	}
}

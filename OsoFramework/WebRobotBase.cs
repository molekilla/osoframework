// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OsoFramework.Http;
using System.Configuration;
using log4net;
using log4net.Config;
using System.Reflection;
using OsoFrameworkManager;
using OsoFramework.Management;


namespace OsoFramework
{
    public class  WebRobotBase : HttpCommand
    {
        protected string ENCODING_ISO = "ISO-8859-1";
        protected string ENCODING_ISO_15 = "ISO-8859-15";
        protected string ENCODING_UTF8 = System.Text.Encoding.UTF8.EncodingName;
        private static ILog Log = null;
        List<HttpSettings> steps = new List<HttpSettings>();
        LogServiceClient logclient = new LogServiceClient();
        WebRobotManagementStatus status = WebRobotManagementStatus.IDLE;
        public event EventHandler OnStatusChanged;

        public WebRobotBase()
        {
            
            XmlConfigurator.Configure();
            
            if (Log == null)
            {
                var methodBase = MethodBase.GetCurrentMethod();
                Type t = methodBase.DeclaringType;
                Log = LogManager.GetLogger(t);
            }
            
            OnStatusChanged += new EventHandler(WebRobotBase_OnStatusChanged);
        }

        void WebRobotBase_OnStatusChanged(object sender, EventArgs e)
        {
            // LogServiceClient client = new LogServiceClient();
            logclient.PingStatus(Name, Status);
            if ( Status == WebRobotManagementStatus.STOPPED 
                || Status == WebRobotManagementStatus.ERROR )
            Status = WebRobotManagementStatus.IDLE;
        }

        public void SetConnectionString(string connectionString)
        {
            this.DatabaseRepository.LoadRepository(connectionString);
        }

        // IWebRobot implementation	
        public string Name
        {
            get;
            set;
        }

        public WebRobotManagementStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnStatusChanged(this, EventArgs.Empty);
            }
        }
        public IDataRepository DatabaseRepository
        {
            get;
            set;
        }
        protected string GetPercentageFrom(int page, int total)
        {
            return (Decimal.Divide(page, total) * 100).ToString("0.00") + "%";
        }
        public void Info(object data)
        {
            
            Console.WriteLine(data);
            logclient.WriteLog(
                new WebRobotStreamLogLine[] 
                {
                   new WebRobotStreamLogLine
                   {
                       Line = data.ToString(),
                       RobotName = this.Name,
                       Timestamp = DateTime.Now
                   }
                });
            Log.Info(data);
            
        }
        public void Error(object err)
        {
            Console.WriteLine(err.ToString());
            Log.Error(err);
            logclient.WriteLog(
                new WebRobotStreamLogLine[] 
                {
                   new WebRobotStreamLogLine
                   {
                       Line = err.ToString(),
                       RobotName = this.Name,
                       Timestamp = DateTime.Now
                   }
                });
        }
        public void InitializeHttpCommand()
        {
            // start http client
            HttpWebClient<HttpCommand> httpClient = new HttpWebClient<HttpCommand>();

            try
            {
                int idleTime = 1;
                int connectionLimit = Int32.Parse(ConfigurationManager.AppSettings["OsoFx.RobotConnectionLimit"]);
                int maxServicePoints = Int32.Parse(ConfigurationManager.AppSettings["OsoFx.RobotMaxServicePoints"]);
                httpClient.SetServicePointSettings(connectionLimit, idleTime * 60 * 1000, maxServicePoints);
            }
            catch
            {
                httpClient.SetServicePointSettings(30, 2 * 60 * 1000, 15);
            }

            this.SetHttpClient(httpClient);

            try
            {
                this.KeepAlive = bool.Parse(ConfigurationManager.AppSettings["OsoFx.RobotKeepAlive"]);
            }
            catch
            {
                this.KeepAlive = false;
            }

            NavigationConfiguration config =
                (NavigationConfiguration)ConfigurationManager.GetSection("OsoFx.NavigationSteps");

           

            foreach (var step in config.NavigationSteps.Cast<HttpNavigationStep>() )
            {
                // load links
                Navigation.AddLast(step);
            }

        }
    }
}

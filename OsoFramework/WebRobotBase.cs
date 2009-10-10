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


namespace OsoFramework
{
    public class  WebRobotBase : HttpCommand
    {
        protected string ENCODING_ISO = "ISO-8859-1";
        protected string ENCODING_ISO_15 = "ISO-8859-15";
        protected string ENCODING_UTF8 = System.Text.Encoding.UTF8.EncodingName;
        private static ILog Log = LogManager.GetLogger(typeof(WebRobotBase));
        List<HttpSettings> steps = new List<HttpSettings>();


        public WebRobotBase()
        {
            XmlConfigurator.Configure();
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
        protected void Print(object data)
        {
            Console.WriteLine(data);
            Log.Info(data);
        }
        protected void Error(object err)
        {
            Log.Error(err);
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

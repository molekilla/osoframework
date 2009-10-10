// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Sgml;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace OsoFramework.Http
{
    public delegate void ValidateResultEventHandler(string response, IHttpCommand context);

    public partial class HttpCommand : IHttpCommand
    {
        bool keepAlive = false;
        string referer = string.Empty;
        Uri responseUri;
        WebHeaderCollection webHeaders;
        string statusDescription;
        HttpStatusCode statusCode;
        HttpSettings settings = null;
        IHttpClient currentClient;
        private CookieCollection cookies;
        bool doWriteStream;
        byte[] writeStream;
        string method;
        private NetworkCredential basicAuthentication = null;
        ValidateResultEventHandler validateMethod;

        HttpNavigation navigation;

        public HttpNavigation Navigation
        {
            get
            {
                return navigation;
            }
            set
            {
                navigation = value;
            }
        }
        /// <summary>
        /// Creates a new HttpCommand context.
        /// </summary>
        public HttpCommand()
        {
            navigation = new HttpNavigation(this);
        }

        public void CreateDefaultHttpClient()
        {
            HttpWebClient<HttpCommand> httpClient = new HttpWebClient<HttpCommand>();
            httpClient.SetServicePointSettings(20, 100 * 1000, 10);
            currentClient = httpClient;
        }

        public void SetHttpClient(IHttpClient client)
        {
            currentClient = client;
        }


        public HttpCommand(HttpSettings settings, string actionMethod, bool doWriteMessage, string writeMessage,ValidateResultEventHandler validateMethod)
        {
            CurrentHttpSettings = settings;
            method = actionMethod;
            doWriteStream = doWriteMessage;
            this.ValidateMethod = validateMethod;
            if (writeMessage != null)
                if (writeMessage.Length > 0)
                    WriteStreamMessage(writeMessage);

        }

        public byte[] ReadWebBinaryResource(string url)
        {
            WebClient webclient = new WebClient();
            byte[] data = webclient.DownloadData(url);
            return data;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public string Read(HttpSettings settings)
        {
            string response_output = string.Empty;
            
            try
            {

                this.Get(settings,
                    (response, command) =>
                    {
                        response_output = response;

                    }).Execute();

            }
            catch ( Exception ex )
            {
                response_output += ex.ToString();
            }

            return response_output;
        }



        /// <summary>
        /// Posts the document for URL.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="data">The data.</param>
        /// <param name="preParser">The pre parser.</param>
        /// <returns></returns>
        public XDocument PostDocumentForUrl(HttpSettings settings,string data, Func<string, string> preParser)
        {
            XDocument document = null;

            this.Post(settings, data, (response, sender) =>
            {
                string result = preParser(response);

                if (!TryParseToXml(result, out document))
                {
                    document = null;
                }
            }).Execute();

            return document;
        }


        /// <summary>
        /// Gets the document for URL.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preParser">The pre parser.</param>
        /// <returns></returns>
        public XDocument ReadXml(HttpSettings settings, Func<string, string> preParser)
        {
            XDocument document = null;

            this.Get(settings, (response, sender) => 
            {
                string result = preParser(response);
                
                if (!TryParseToXml(result, out document))
                {
                    document = null;
                }
            } ).Execute();

            return document;
        }

        /// <summary>
        /// Gets the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="validateHandler">The validate method.</param>
        /// <returns></returns>
        public HttpCommand Get(HttpSettings settings, ValidateResultEventHandler validateHandler)
        {
            return Create(settings, "GET", false, null, validateHandler);
        }

        /// <summary>
        /// Posts the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="validateHandler">The validate handler.</param>
        /// <returns></returns>
        public HttpCommand Post(HttpSettings settings,string postData, ValidateResultEventHandler validateHandler)
        {
            settings.ContentType = "application/x-www-form-urlencoded";
            return Create(settings, "POST", true, postData, validateHandler);
        }

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="actionMethod">The action method.</param>
        /// <param name="doWriteMessage">if set to <c>true</c> [do write message].</param>
        /// <param name="writeMessage">The write message.</param>
        /// <param name="validateMethod">The validate method.</param>
        /// <returns></returns>
        public HttpCommand Create(HttpSettings settings , string actionMethod, bool doWriteMessage, string writeMessage, ValidateResultEventHandler validateMethod)
        {
            this.Method = actionMethod;
            this.CurrentHttpSettings = settings;
            this.DoWriteStream = doWriteMessage;
            this.ValidateMethod = validateMethod;

            if (writeMessage != null)
                if (writeMessage.Length > 0)
                    WriteStreamMessage(writeMessage);

            return this;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal void Execute()
        {
            if (currentClient == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                currentClient.SendServiceRequest(this);
            }
        }

        /// <summary>
        /// Executes the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        internal void Execute(HttpSettings settings)
        {
            if (currentClient == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.CurrentHttpSettings = settings;
                currentClient.SendServiceRequest(this);
            }
        }

        /// <summary>
        /// Asyncs the execute.
        /// </summary>
        public void AsyncExecute()
        {
            if (currentClient == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                currentClient.SendServiceRequestAsync(this, new object());
            }
        }

        /// <summary>
        /// Executes the with.
        /// </summary>
        /// <param name="client">The client.</param>
        public void ExecuteWith(IHttpClient client)
        {
            client.SendServiceRequest(this);
        }

        /// <summary>
        /// Asyncs the execute with.
        /// </summary>
        /// <param name="client">The client.</param>
        public void AsyncExecuteWith(IHttpClient client)
        {
            client.SendServiceRequestAsync(this, new object());
        }
        /// <summary>
        /// Gets or sets the validate method.
        /// </summary>
        /// <value>The validate method.</value>
        public ValidateResultEventHandler ValidateMethod
        {
            get { return validateMethod; }
            set { validateMethod = value; }
        }

      
      
        /// <summary>
        /// Writes the stream message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteStreamMessage(string message)
        {
            StreamMessage = Encoding.UTF8.GetBytes(message);
        }

        /// <summary>
        /// Sets the basic authentication.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public void SetBasicAuthentication(string username, string password, string domain)
        {
            BasicAuthentication = new NetworkCredential(username, password, domain);
        }

        /// <summary>
        /// Removes the basic authentication.
        /// </summary>
        public void RemoveBasicAuthentication()
        {
            BasicAuthentication = null;
        }


        /// <summary>
        /// Tries the parse to XML.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public bool TryParseToXml(string response, out XDocument document)
        {
            string result = SgmlConverter.ParseHtml(response);
            if (result.Length == 0)
            {
                document = null;
                return false;
            }
            else
            {
                document = XDocument.Parse(result);
                return true;
            }
        }

        /// <summary>
        /// Tries the parse to XML.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public XDocument ParseToXml(string response, Func<string,string> preparse)
        {
            XDocument document = null;
            string newResponse = preparse(response);
            string result = SgmlConverter.ParseHtml(newResponse);
            if (result.Length == 0)
            {
                document = null;
            }
            else
            {
                document = XDocument.Parse(result);
            }
            return document;
        }

        /// <summary>
        /// Fills the query.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
       public static string FillQuery(string formatString, params string[] values)
        {
            StringBuilder builder = new StringBuilder();
            return builder.AppendFormat(formatString, values).ToString();
        }

       public CookieContainer ClientCookieContainer
       {
           get
           {
               return currentClient.CookieContainer;
           }

       }

       #region IServiceRequestContext Members

       public HttpSettings CurrentHttpSettings
       {
           get
           {
               return settings;
           }
           set
           {
               settings = value;
           }
       }
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public string Method
        {
            get
            {
                return method;
            }
            set
            {
                method = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [do write stream].
        /// </summary>
        /// <value><c>true</c> if [do write stream]; otherwise, <c>false</c>.</value>
        public bool DoWriteStream
        {
            get
            {
                return doWriteStream;
            }
            set
            {
                doWriteStream = value;
            }
        }

        /// <summary>
        /// Gets or sets the stream message.
        /// </summary>
        /// <value>The stream message.</value>
        public byte[] StreamMessage
        {
            get
            {
                return writeStream;
            }
            set
            {
                writeStream = value;
            }
        }

        public NetworkCredential BasicAuthentication
        {
            get { return basicAuthentication; }
            set { basicAuthentication = value; }
        }

        public CookieCollection Cookies
        {
            get { return cookies; }
            set { cookies = value; }
        }

        public HttpStatusCode StatusCode
        {
            get { return StatusCode; }
            set { statusCode = value; }
        }

        public string StatusDescription
        {
            get { return statusDescription; }
            set { statusDescription = value; }
        }

        public Uri ResponseUri 
        {
            get { return responseUri; }
            set { responseUri = value; }
        }
        public WebHeaderCollection WebHeaders
        {
            get { return webHeaders; }
            set { webHeaders = value; }
        }
        public string Referer
        {
            get { return referer; }
            set { referer = value; }
        }

        public bool KeepAlive
        {
            get { return keepAlive; }
            set { keepAlive = value; }
        }

        #endregion
    }
}

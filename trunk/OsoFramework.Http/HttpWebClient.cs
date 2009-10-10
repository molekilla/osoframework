// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
// HttpLightClient by Rogelio Morrell
// Ecyware
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace OsoFramework.Http
{
    public delegate void SendServiceRequestCompletedEventHandler(object sender, SendServiceRequestEventArgs e);

    public class SendServiceRequestEventArgs : AsyncCompletedEventArgs
    {
        bool success;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SendServiceRequestEventArgs"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get {
                RaiseExceptionIfNecessary();
                return success; 
            }
            set { success = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendServiceRequestEventArgs"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="e">The e.</param>
        /// <param name="canceled">if set to <c>true</c> [canceled].</param>
        /// <param name="state">The state.</param>
        public SendServiceRequestEventArgs(bool success, Exception e, bool canceled, object state) : base(e, canceled, state)
        {
            this.success = success;
        }
    }

    public class HttpWebClient<T> : IHttpClient
        where T : HttpCommand, new()
    {
        private CookieContainer coookieContainer = new CookieContainer();
        private delegate void WorkerEventHandler(IHttpCommand context, AsyncOperation asyncOp);
        private SendOrPostCallback onCompletedDelegate;
        private HybridDictionary userStateToLifetime = new HybridDictionary();
        public event SendServiceRequestCompletedEventHandler SendServiceRequestCompleted;
        // TODO: Change this
        private string _userAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT)";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpLightClient"/> class.
        /// </summary>
        public HttpWebClient()
        {
            InitializeDelegates();
        }

        public CookieContainer CookieContainer
        {
            get
            {
                return coookieContainer;
            }
            set
            {
                coookieContainer = value;
            }
        }
        

        public T CreateHttpCommand()
        {
            T newObject = new T();
            newObject.SetHttpClient(this);
            return newObject;
        }
        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private void InitializeDelegates()
        {
            onCompletedDelegate = new SendOrPostCallback(Completed);
        }

        #region Method Helpers        
        /// <summary>
        /// Creates the service request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private HttpWebRequest CreateServiceRequest(IHttpCommand context)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(context.CurrentHttpSettings.Query);
            return request;
        }

        /// <summary>
        /// To Zulu time format.
        /// </summary>
        /// <param name="date"> The datetime.</param>
        /// <returns> Returns the datetime in zulu string format.</returns>
        private string ToUtcTime(DateTime date)
        {
            string zulu = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            //string zulu = date.ToUniversalTime().ToString("s", DateTimeFormatInfo.InvariantInfo) + "Z";
            return zulu;
        }
        #endregion
        #region Async
        /// <summary>
        /// Completeds the specified operation state.
        /// </summary>
        /// <param name="operationState">State of the operation.</param>
        private void Completed(object operationState)
        {
            SendServiceRequestEventArgs e = operationState as SendServiceRequestEventArgs;
            OnCompletedChanged(e);
        }

        protected void OnCompletedChanged(SendServiceRequestEventArgs e)
        {
            if (SendServiceRequestCompleted != null)
            {
                SendServiceRequestCompleted(this, e);
            }
        }

        /// <summary>
        /// Asyncs the task canceled.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        private bool AsyncTaskCanceled(object state)
        {
            return (userStateToLifetime[state] == null);
        }
        /// <summary>
        /// Cancels the async.
        /// </summary>
        /// <param name="state">The state.</param>
        public void CancelAsync(object state)
        {
            AsyncOperation asyncOp = userStateToLifetime[state] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(state);
                }
            }
        }

        /// <summary>
        /// Sends the service request async wrapper.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="asyncOp">The async op.</param>
        private void SendServiceRequestAsyncWrapper(IHttpCommand context, AsyncOperation asyncOp)
        {
            bool success = false;
            Exception e = null;
            if (!AsyncTaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    success = SendServiceRequest(context);
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            CompletionMethod(success, e, AsyncTaskCanceled(asyncOp.UserSuppliedState), asyncOp);
        }

        /// <summary>
        /// Completions the method.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="exception">The exception.</param>
        /// <param name="canceled">if set to <c>true</c> [canceled].</param>
        /// <param name="asyncOp">The async op.</param>
        private void CompletionMethod(bool success, Exception exception, bool canceled, AsyncOperation asyncOp)
        {
            if (!canceled)            
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }

            asyncOp.PostOperationCompleted(onCompletedDelegate,
                new SendServiceRequestEventArgs(success,exception, canceled,asyncOp.UserSuppliedState));
        }

        #endregion
        #region Main Methods

        /// <summary>
        /// Sends the service request async.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="state">The state.</param>
        public void SendServiceRequestAsync(IHttpCommand context, object state)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(state);

            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(state))
                {
                    throw new ArgumentException("state must be unique for async operation", "state");
                }

                userStateToLifetime[state] = asyncOp;
            }

            WorkerEventHandler workerDelegate = new WorkerEventHandler(SendServiceRequestAsyncWrapper);
            workerDelegate.BeginInvoke(context, asyncOp, null,null);

        }



        /// <summary>
        /// Sends the service request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool SendServiceRequest(IHttpCommand context)
        {
            bool success = false;
            SetSslConfiguration();

            // Create user task folder url
            System.Net.HttpWebRequest req = CreateServiceRequest(context);

            // Setting security properties
            // Validate if uses BasicAuthentication
            if (context.BasicAuthentication == null)
            {
                if (context.CurrentHttpSettings.UseNetworkCredentials)
                {
                    req.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    req.Credentials = CredentialCache.DefaultCredentials;
                }
            } else
            {
                System.Net.CredentialCache credentialCache = new System.Net.CredentialCache();
                credentialCache.Add(req.RequestUri,"Basic", context.BasicAuthentication);
                req.Credentials = credentialCache;
            }

            // HttpClient properties
            // req.Headers.Add("Cookie", cookies);
            req.AllowAutoRedirect = true;
            req.Method = context.Method;
            req.ContentType = context.CurrentHttpSettings.ContentType;
            req.AutomaticDecompression = DecompressionMethods.GZip;
            req.Referer = context.Referer;
            req.KeepAlive = context.KeepAlive;

            // Cookie handling
            if (coookieContainer.Count > 0)
            {
                if (context.Cookies != null)
                {
                    coookieContainer.Add(context.Cookies);
                }
                req.CookieContainer = coookieContainer;
            }

            // Set the user-agent headers
            req.UserAgent = _userAgent;

            if ( context.CurrentHttpSettings.UserAgent.Length > 0 )
            {
                req.UserAgent = context.CurrentHttpSettings.UserAgent;
            }

            // Write message
            if (context.DoWriteStream)
            {
                byte[] bytes = context.StreamMessage;
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }

            // Get web response
            HttpWebResponse response = null;
            string messageResult = string.Empty;

            try
            {
                //req.Timeout = 1000 * 60;
                response = (HttpWebResponse)req.GetResponse();
                using (Stream resStream = response.GetResponseStream())
                {
                    Encoding encode = System.Text.Encoding.GetEncoding(context.CurrentHttpSettings.ResponseEncoding);
                    using (StreamReader sr = new StreamReader(resStream, encode))
                    {
                        messageResult = sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                if (response != null)
                    response.Close();
                throw;
            }
            
            // Validate set-cookie
            if ( response.Headers["Set-Cookie"] != null)
            {
                try
                {
                    string[] cookie = response.Headers["Set-Cookie"].Split(';');
                    coookieContainer.Add(
                        new Cookie(cookie[0].Split('=')[0], cookie[0].Split('=')[1], "/", response.ResponseUri.Authority)
                        );
                }
                catch
                {
                    // ignore
                }
            }

            coookieContainer.Add(response.Cookies);

            context.WebHeaders = response.Headers;
            context.ResponseUri = response.ResponseUri;
            context.StatusCode = response.StatusCode;
            context.StatusDescription = response.StatusDescription;
            
            response.Close();


            // Validate result
            try
            {
                if (context.ValidateMethod != null)
                {
                    context.ValidateMethod(messageResult, context);
                }
                success = true;
            }
            catch 
            {
                success = false;
            }
            

            return success;
        }       
        #endregion
        #region SSL Settings
        /// <summary>
        /// SSL Validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns> Returns true if valid, else false.</returns>
        private static bool ValidateServerCert(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }

        /// <summary>
        /// Sets the SSL configuration.
        /// </summary>
        protected void SetSslConfiguration()
        {
            // Handle any certificate errors on the certificate from the server.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // TODO:
			//ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
        }
        #endregion
        #region ServicePointManager settings
        public void SetServicePointSettings(int connectionLimit, int idleTime, int maxServicePoints)
        {
            ServicePointManager.DefaultConnectionLimit = connectionLimit;
            ServicePointManager.MaxServicePointIdleTime = idleTime;
            ServicePointManager.MaxServicePoints = maxServicePoints;
            
        }
        #endregion
    }
}

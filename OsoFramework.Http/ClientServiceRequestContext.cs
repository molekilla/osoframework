// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OsoFramework.Http
{

    

    public class ClientServiceRequestContext : IServiceRequestContext
    {
        string method;
        string query;
        bool doWriteStream;
        byte[] writeStream;
        string contentType;
        bool isWindowsSecurity;
        private NetworkCredential basicAuthentication = null;
        ValidateResultEventHandler validateMethod;


        /// <summary>
        /// Creates a new ClientServiceRequest context.
        /// </summary>
        public ClientServiceRequestContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientServiceRequestContext"/> class.
        /// </summary>
        /// <param name="actionMethod">The action method.</param>
        /// <param name="getQuery">The get query.</param>
        /// <param name="doWriteMessage">if set to <c>true</c> [do write message].</param>
        /// <param name="writeMessage">The write message.</param>
        public ClientServiceRequestContext(string actionMethod, string getQuery, bool doWriteMessage, string writeMessage)
        {
            method = actionMethod;
            query = getQuery;
            doWriteStream = doWriteMessage;

            if (writeMessage != null)
                if (writeMessage.Length > 0)
                    WriteStreamMessage(writeMessage);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientServiceRequestContext"/> class.
        /// </summary>
        /// <param name="actionMethod">The action method.</param>
        /// <param name="getQuery">The get query.</param>
        /// <param name="doWriteMessage">if set to <c>true</c> [do write message].</param>
        /// <param name="writeMessage">The write message.</param>
        /// <param name="contentTypeString">The content type string.</param>
        /// <param name="useNetworkCredentials">if set to <c>true</c> [use network credentials].</param>
        /// <param name="validateMethod">The validate method.</param>
        public ClientServiceRequestContext(string actionMethod, string getQuery, bool doWriteMessage, string writeMessage, string contentTypeString, bool useNetworkCredentials, ValidateResultEventHandler validateMethod)
            : this(actionMethod, getQuery, doWriteMessage, writeMessage)
        {
            contentType = contentTypeString;
            isWindowsSecurity = useNetworkCredentials;
            this.ValidateMethod = validateMethod;
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
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [use network credentials].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use network credentials]; otherwise, <c>false</c>.
        /// </value>
        public bool UseNetworkCredentials
        {
            get { return isWindowsSecurity; }
            set { isWindowsSecurity = value; }
        }
        /// <summary>
        /// Writes the stream message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteStreamMessage(string message)
        {
            StreamMessage = Encoding.UTF8.GetBytes(message);
        }

        public void SetBasicAuthentication(string username, string password, string domain)
        {
            BasicAuthentication = new NetworkCredential(username, password, domain);
        }

        public void RemoveBasicAuthentication()
        {
            BasicAuthentication = null;
        }

        #region IServiceRequestContext Members

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
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
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

        #endregion
    }
}

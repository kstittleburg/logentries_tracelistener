using System;
using System.Diagnostics;
using System.Linq;
using LogentriesCore.Net;
using Microsoft.WindowsAzure;

namespace LogEntries.TraceListener
{
    public class LogentriesListener : SingleLineListener
    {
        private readonly AsyncLogger logentriesAsync;
        private readonly static object LockObject = new object();

        public LogentriesListener()
            : this("")
        {
        }

        public LogentriesListener(string token)
        {
            logentriesAsync = new AsyncLogger();
            Token = token;
        }

        /// <summary>
        /// Adds a new LogentriesListener to Trace.Listeners if none exists already
        /// </summary>
        /// <param name="tokenConfigurationKey">Configuration key (app.config or cloud cscfg) to read logentries.com token from</param>
        /// <remarks>Assumes you will only ever have 1 LogentriesListener within your app domain; of an existing listener is found, just the token is updated.</remarks>
        public static void GlobalInitialize(string tokenConfigurationKey = "Logentries.Token")
        {
            lock (LockObject)
            {
                var token = CloudConfigurationManager.GetSetting(tokenConfigurationKey);
                var existingListener = Trace.Listeners.OfType<LogentriesListener>().FirstOrDefault();

                if (existingListener != null)
                {
                    existingListener.Token = token;
                }
                else
                {
                    Trace.Listeners.Add(new LogentriesListener(token));    
                }
            }
        }

        #region attributeMethods

        /* Option to set LOGENTRIES_TOKEN programmatically or in appender definition. */
        public string Token
        {
            get
            {
                return logentriesAsync.getToken();
            }
            set
            {
                logentriesAsync.setToken(value);
            }
        }

        /* Option to set LOGENTRIES_ACCOUNT_KEY programmatically or in appender definition. */
        public String AccountKey
        {
            get
            {
                return logentriesAsync.getAccountKey();
            }
            set
            {
                logentriesAsync.setAccountKey(value);
            }
        }

        /* Option to set LOGENTRIES_LOCATION programmatically or in appender definition. */
        public String Location
        {
            get
            {
                return logentriesAsync.getLocation();
            }
            set
            {
                logentriesAsync.setLocation(value);
            }
        }

        /* Set to true to always flush the TCP stream after every written entry. */
        public bool ImmediateFlush
        {
            get
            {
                return logentriesAsync.getImmediateFlush();
            }
            set
            {
                logentriesAsync.setImmediateFlush(value);
            }
        }

        /* Debug flag. */
        public bool Debug
        {
            get
            {
                return logentriesAsync.getDebug();
            }
            set
            {
                logentriesAsync.setDebug(value);
            }
        }


        /* Set to true to use HTTP PUT logging. */
        public bool UseHttpPut
        {
            get
            {
                return logentriesAsync.getUseHttpPut();
            }
            set
            {
                logentriesAsync.setUseHttpPut(value);
            }
        }

        /* This property exists for backward compatibility with older configuration XML. */
        [Obsolete("Use the UseHttpPut property instead.")]
        public bool HttpPut
        {
            get
            {
                return logentriesAsync.getUseHttpPut();
            }
            set
            {
                logentriesAsync.setUseHttpPut(value);
            }
        }


        /* Set to true to use SSL with HTTP PUT logging. */
        public bool UseSsl
        {
            get
            {
                return logentriesAsync.getUseSsl();
            }
            set
            {
                logentriesAsync.setUseSsl(value);
            }
        }

        /* Is using DataHub parameter flag. - set to true to use DataHub server */
        public bool IsUsingDataHub
        {
            get
            {
                return logentriesAsync.getIsUsingDataHab();
            }
            set
            {
                logentriesAsync.setIsUsingDataHub(value);
            }
        }

        /* DataHub server address */
        public String DataHubAddr
        {
            get
            {
                return logentriesAsync.getDataHubAddr();
            }
            set
            {
                logentriesAsync.setDataHubAddr(value);
            }
        }

        /* DataHub server port */
        public int DataHubPort
        {
            get
            {
                return logentriesAsync.getDataHubPort();
            }
            set
            {
                logentriesAsync.setDataHubPort(value);
            }
        }

        /* Switch that defines whether add host name to the log message */
        public bool LogHostname
        {
            get
            {
                return logentriesAsync.getUseHostName();
            }
            set
            {
                logentriesAsync.setUseHostName(value);
            }
        }

        /* User-defined host name. If empty the library will try to obtain it automatically */
        public String HostName
        {
            get
            {
                return logentriesAsync.getHostName();
            }
            set
            {
                logentriesAsync.setHostName(value);
            }
        }

        /* User-defined log message ID */
        public String LogID
        {
            get
            {
                return logentriesAsync.getLogID();
            }
            set
            {
                logentriesAsync.setLogID(value);
            }
        }

        /* This property exists for backward compatibility with older configuration XML. */
        [Obsolete("Use the UseSsl property instead.")]
        public bool Ssl
        {
            get
            {
                return logentriesAsync.getUseSsl();
            }
            set
            {
                logentriesAsync.setUseSsl(value);
            }
        }

        #endregion

        public override void WriteLine(string message)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message, null, null, null))
                return;

            logentriesAsync.AddLine(message);
        }
    }
}

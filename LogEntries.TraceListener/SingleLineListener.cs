using System;
using System.Diagnostics;
using System.Globalization;

namespace LogEntries.TraceListener
{
    /// <summary>
    /// Implements a <see cref="TraceListener"/>, before writes all messages as a single line instead of multiple lines
    /// </summary>
    public abstract class SingleLineListener : System.Diagnostics.TraceListener
    {
        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void Write(string message, string category)
        {
            WriteLine(message, category);
        }

        public override void Write(object o)
        {
            WriteLine(o == null ? "" : o.ToString());
        }

        public override void Write(object o, string category)
        {
            WriteLine(o, category);
        }

        public abstract override void WriteLine(string message);


        public override void WriteLine(string message, string category)
        {
            if (category == null)
                WriteLine(message);
            else
                WriteLine(category + ": " + (message));
        }

        public override void WriteLine(object o)
        {
            WriteLine(o == null ? "" : o.ToString());
        }

        public override void WriteLine(object o, string category)
        {
            if (category == null)
                WriteLine(o);
            else
                WriteLine(o == null ? "" : o.ToString(), category);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format,
            params object[] args)
        {
            this.TraceEvent(eventCache, source, eventType, id, string.Format(format, args));
        }

        public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            var logMessage = String.Format(CultureInfo.InvariantCulture, "{0} {1}: ", source, MapLogLevel(eventType))
                             + message;

            WriteLine(logMessage);
        }

        private static string MapLogLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return "TRACE";
                case TraceEventType.Verbose:
                    return "DEBUG";
                case TraceEventType.Information:
                    return "INFO";
                case TraceEventType.Warning:
                    return "WARN";
                case TraceEventType.Error:
                    return "ERROR";
                case TraceEventType.Critical:
                    return "FATAL";
                default:
                    return "TRACE";
            }
        }
    }
}

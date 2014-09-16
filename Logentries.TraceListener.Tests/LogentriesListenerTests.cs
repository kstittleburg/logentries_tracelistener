using System;
using System.Configuration;
using System.Diagnostics;
using LogEntries.TraceListener;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logentries.TraceListener.Tests
{
    [TestClass]
    public class LogentriesListenerTests
    {
        [TestMethod]
        public void SimpleWriteTest()
        {
            var logentriesListener = new LogentriesListener
            {
                Token = ConfigurationManager.AppSettings["Logentries.Token"]
            };

            Trace.Listeners.Add(logentriesListener);
            Trace.WriteLine("Testing line written to logentries.com");
            Trace.Flush();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10)); // sleep a bit to ensure our message makes it out to logentries
            // manually verify message was written to logentries
        }
    }
}

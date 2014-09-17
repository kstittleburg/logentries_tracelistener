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
        public void LogentriesWriteTest()
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

        [TestMethod]
        public void TokenConstructorTest()
        {
            var logentriesListener = new LogentriesListener(ConfigurationManager.AppSettings["Logentries.Token"]);

            Assert.AreEqual(ConfigurationManager.AppSettings["Logentries.Token"], logentriesListener.Token);
        }

        [TestMethod]
        public void InitializeListenerTest()
        {
            Trace.Listeners.Clear();

            Assert.AreEqual(0, Trace.Listeners.Count);
            
            LogentriesListener.GlobalInitialize();
            Assert.AreEqual(1, Trace.Listeners.Count);
            var logEntriesListener = Trace.Listeners[0] as LogentriesListener;
            Assert.IsNotNull(logEntriesListener);
            Assert.AreEqual(ConfigurationManager.AppSettings["Logentries.Token"], logEntriesListener.Token);

            // second initialize should not add a new listener, but will update token
            LogentriesListener.GlobalInitialize("Logentries.Token2");
            Assert.AreEqual(1, Trace.Listeners.Count);
            logEntriesListener = Trace.Listeners[0] as LogentriesListener;
            Assert.AreEqual(ConfigurationManager.AppSettings["Logentries.Token2"], logEntriesListener.Token);
        }
    }
}

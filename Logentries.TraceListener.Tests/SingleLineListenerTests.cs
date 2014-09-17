using System.Diagnostics;
using LogEntries.TraceListener;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logentries.TraceListener.Tests
{
    [TestClass]
    public class SingleLineListenerTests
    {
        [TestMethod]
        public void SimpleWriteTest()
        {
            var listener = new MockListener();

            Trace.Listeners.Add(listener);

            Trace.TraceInformation("INFO INFO INFO");
            Assert.AreEqual("vstest.executionengine.x86.exe INFO: INFO INFO INFO", listener.EntryWritten);

            Trace.TraceWarning("WARNING WARNING WARNING");
            Assert.AreEqual("vstest.executionengine.x86.exe WARN: WARNING WARNING WARNING", listener.EntryWritten);

            Trace.TraceError("ERROR ERROR ERROR");
            Assert.AreEqual("vstest.executionengine.x86.exe ERROR: ERROR ERROR ERROR", listener.EntryWritten);

            Trace.Write("I WRITE");
            Assert.AreEqual("I WRITE", listener.EntryWritten);

            Trace.Write("I WRITE", "CATEGORY");
            Assert.AreEqual("CATEGORY: I WRITE", listener.EntryWritten);

            Trace.WriteLine("I WRITE A LINE");
            Assert.AreEqual("I WRITE A LINE", listener.EntryWritten);

            Trace.WriteLine("I WRITE A LINE", "CATEGORY");
            Assert.AreEqual("CATEGORY: I WRITE A LINE", listener.EntryWritten);
        }

        // TODO: Additional overloads and ShouldTrace functionality could be tested

        public class MockListener : SingleLineListener
        {
            public string EntryWritten { get; internal set; }

            public override void WriteLine(string message)
            {
                EntryWritten = message;
            }
        }

    }
}

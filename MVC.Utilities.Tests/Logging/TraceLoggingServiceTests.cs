using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MVC.Utilities.Logging;
using NUnit.Framework;

namespace MVC.Utilities.Tests.Logging
{
    [TestFixture(Description = "Tests aimed to ensure that our trace logging service works as expected")]
    public class TraceLoggingServiceTests
    {
        private TraceLoggingService _traceLogging;
        private FakeTraceListener _fakeTraceListener;

        public TraceLoggingServiceTests()
        {
            _traceLogging = new TraceLoggingService();
            _fakeTraceListener = new FakeTraceListener();
        }

        #region Setup / Teardown

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //Clear all of the current trace listeners
            Trace.Listeners.Clear();

            //Listen to just our fake trace listener
            Trace.Listeners.Add(_fakeTraceListener);
        }

        [TearDown]
        public void TearDown()
        {
            //Clear all of the messages collected in our Fake trace listener
            _fakeTraceListener.Messages.Clear();
            _fakeTraceListener.LastObservedCategory = String.Empty;
        }

        #endregion

        #region Tests

        [Test(Description = "If we're using LogEntryType.Success, the ToString() of that should read 'Success'")]
        public void ShouldMapLogEventTypeToString()
        {
            var logEntryType = LogEntryType.Success;
            Assert.AreEqual("Success", logEntryType.ToString());
        }

        [Test(Description = "Should log a simple message to our trace listener")]
        public void ShouldLogToTrace()
        {
            var message = "Hi there!";
            _traceLogging.LogMessage(message);

            Assert.IsTrue(_fakeTraceListener.Messages.All(x => x.Equals(message)));

            _traceLogging.LogMessage(message, LogEntryType.Success);

            //Setting a category flag should not impact the contents of the message itself
            Assert.AreEqual(2, _fakeTraceListener.Messages.Count);
            Assert.IsTrue(_fakeTraceListener.Messages.All(x => x.Equals(message)));
            Assert.AreEqual(_fakeTraceListener.LastObservedCategory, "Success", string.Format("Expected 'Success', not {0}", _fakeTraceListener.LastObservedCategory));
        }

        #endregion
    }
}

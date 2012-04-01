using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MVC.Utilities.Logging;

namespace MVC.Utilities.Tests.Logging
{
    public class FakeTraceListener : TraceListener
    {
        public IList<string> Messages { get; private set; }
        public string LastObservedCategory { get; set; }

        public FakeTraceListener()
        {
            Messages = new List<string>();
        }

        #region Overrides of TraceListener

        public override void WriteLine(object o, string category)
        {
            WriteLine(o.ToString(), category);
        }

        public override void Write(string message)
        {
            Messages.Add(message);
        }

        public override void WriteLine(string message, string category)
        {
            Messages.Add(message);
            LastObservedCategory = category;
        }

        public override void WriteLine(string message)
        {
            Messages.Add(message);
        }

        #endregion
    }
}

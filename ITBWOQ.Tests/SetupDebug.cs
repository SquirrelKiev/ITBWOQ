using System.Diagnostics;

namespace ITBWOQ.Tests
{
    [SetUpFixture]
    public class SetupDebug
    {
        [OneTimeSetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();
        }
    }
}

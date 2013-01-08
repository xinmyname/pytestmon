using PyTestMon.Core;
using NUnit.Framework;
using Should;

namespace PyTestMon.Tests
{
    [TestFixture]
    public class IdGeneratorTests
    {
        [Test]
        public void IdIsGeneratedFromModuleAndName()
        {
            string id = IdGenerator.Create("testSomeTestName", "sometests.sometests");

            id.ShouldEqual("__testsometestnamesometestssometests");
        }
    }
}
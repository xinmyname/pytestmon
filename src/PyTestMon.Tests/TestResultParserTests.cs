using System.Collections.Generic;
using System.IO;
using System.Linq;
using PyTestMon.Core;
using NUnit.Framework;
using Should;

namespace PyTestMon.Tests
{
    [TestFixture]
    public class TestResultParserTests
    {
        private static IEnumerable<TestResult> ParseResults(string text)
        {
            var results = new List<TestResult>();
            var reader = new StringReader(text);
            new TestResultParser().Parse(reader, results.Add, rd => { });
            return results;
        }

        private static IEnumerable<TestResultDetails> ParseResultDetails(string text)
        {
            var results = new List<TestResultDetails>();
            var reader = new StringReader(text);
            new TestResultParser().Parse(reader, r => { }, results.Add);
            return results;
        }

        [Test]
        public void NameParsesCorrectly()
        {
            TestResult result = ParseResults("testSomeTestName (sometests.sometests) ... ok").Single();
            result.Name.ShouldEqual("testSomeTestName");
        }

        [Test]
        public void ModuleParsesCorrectly()
        {
            TestResult result = ParseResults("testSomeTestName (sometests.sometests) ... ok").Single();
            result.Module.ShouldEqual("sometests.sometests");
        }

        [Test]
        public void FailResultParsesCorrectly()
        {
            TestResult result = ParseResults("testSomeTestName (sometests.sometests) ... FAIL").Single();
            result.Status.ShouldEqual("FAIL");
        }

        [Test]
        public void ErrorResultParsesCorrectly()
        {
            TestResult result = ParseResults("testSomeTestName (sometests.sometests) ... ERROR").Single();
            result.Status.ShouldEqual("ERROR");
        }

        [Test]
        public void OkResultParsesCorrectly()
        {
            TestResult result = ParseResults("testSomeTestName (sometests.sometests) ... ok").Single();
            result.Status.ShouldEqual("ok");
        }

        [Test]
        public void MultipleLinesYieldMultipleResults()
        {
            int count = ParseResults(
                "testSomeTestName1 (sometests.sometests) ... ok\n" +
                "testSomeTestName2 (sometests.sometests) ... ok\n" +
                "testSomeTestName3 (sometests.sometests) ... ok"
                ).Count();

            count.ShouldEqual(3);
        }
        
        [Test]
        public void BlankLinesDontYieldAResult()
        {
            int count = ParseResults("\n\n\n").Count();

            count.ShouldEqual(0);
        }

        [Test]
        public void ResultDetailsParseCorrectly()
        {
            TestResultDetails details = ParseResultDetails(
                "\n"+
                "======================================================================\n" +
                "FAIL: testFail (sometests.sometests)\n" +
                "----------------------------------------------------------------------\n" +
                "Traceback (most recent call last):\n" +
                "  File \"C:\\sometests\\sometests.py\", line 14, in testFail\n" +
                "    self.assertTrue(False)\n" +
                "AssertionError: False is not True\n" +
                "\n"
                ).Single();

            details.Module.ShouldEqual("sometests.sometests");
            details.Name.ShouldEqual("testFail");

            details.Text.ShouldEqual(
                "Traceback (most recent call last):\n" +
                "  File \"C:\\sometests\\sometests.py\", line 14, in testFail\n" +
                "    self.assertTrue(False)\n" +
                "AssertionError: False is not True\n");
        }

        [Test]
        public void MultipleFailuresYieldMultipleResultDetails()
        {
            int count = ParseResultDetails(
                "\n" +
                "======================================================================\n" +
                "FAIL: testFail1 (sometests.sometests)\n" +
                "----------------------------------------------------------------------\n" +
                "Traceback (most recent call last):\n" +
                "  File \"C:\\sometests\\sometests.py\", line 14, in testFail\n" +
                "    self.assertTrue(False)\n" +
                "AssertionError: False is not True\n" +
                "\n" +
                "======================================================================\n" +
                "FAIL: testFail2 (sometests.sometests)\n" +
                "----------------------------------------------------------------------\n" +
                "Traceback (most recent call last):\n" +
                "  File \"C:\\sometests\\sometests.py\", line 28, in testFail\n" +
                "    self.assertTrue(False)\n" +
                "AssertionError: False is not True\n" +
                "\n"
                ).Count();

            count.ShouldEqual(2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using PyTestMon.Core.Config;

namespace PyTestMon.Core
{
    public class TestFolderMonitor : IDisposable
    {
        private readonly BrowserWriter _browserWriter;
        private readonly TestRunner _testRunner;
        private SetupData _setupData;
        private FileSystemWatcher _watcher;
        private DateTime _lastChanged;
        private bool _success;

        public bool Running { get; private set; }
        public event Action<bool> TestRunComplete = x => { };

        public TestFolderMonitor(BrowserWriter browserWriter, TestRunner testRunner)
        {
            Running = false;
            _browserWriter = browserWriter;
            _testRunner = testRunner;

            _testRunner.Starting += TestRunStarting;
            _testRunner.Finished += TestFinished;
            _testRunner.TestResultReady += TestResultReady;
            _testRunner.TestResultDetailsReady += TestResultDetailsReady;
        }

        public void ChangeSetup(SetupData setupData)
        {
            _setupData = setupData;
        }

        public void Start()
        {
            _browserWriter.Navigate("res://PyTestMon.exe/#23/#101");

            Running = true;
            _lastChanged = DateTime.MinValue;

            _watcher = new FileSystemWatcher
                       {
                           Path = _setupData.ProjectFolder,
                           Filter = "*.py",
                           IncludeSubdirectories = true
                       };

            _watcher.Changed += TestsChanged;
            _watcher.Deleted += TestsChanged;
            _watcher.Created += TestsChanged;
            _watcher.Renamed += TestsChanged;

            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;

            Running = false;
        }

        public void Dispose()
        {
            if (_watcher != null)
                _watcher.Dispose();
        }


        private void TestsChanged(object sender, FileSystemEventArgs e)
        {
            DateTime now = DateTime.Now;

            TimeSpan elapsed = now - _lastChanged;

            // Debounce by 2 seconds - keep your finger off Ctrl-S for moment, would ya?
            if (elapsed.TotalSeconds >= 2)
                _testRunner.RunTests(_setupData);

            _lastChanged = now;
        }

        private void TestRunStarting()
        {
            _browserWriter.ClearBody();
            _success = true;
        }

        private void TestFinished()
        {
            TestRunComplete(_success);
        }


        private void TestResultReady(TestResult result)
        {
            string id = IdGenerator.Create(result.Module, result.Name);

            bool testSucceeded = result.Status == "ok";

            var resultElement = new XElement("div",
                new XAttribute("id", id),
                new XAttribute("class", testSucceeded ? "test passed" : "test failed"),
                new XElement("div", new XAttribute("class", "module"),
                    new XText(result.Module)),
                new XElement("div", new XAttribute("class", "name"),
                    new XText(result.Name)),
                new XElement("div", new XAttribute("class", "result"),
                    new XText(result.Status)));

            if (testSucceeded)
                _browserWriter.AppendToElementId("body", resultElement);
            else
                _browserWriter.PrependToElementId("body", resultElement);

            if (_success && !testSucceeded)
                _success = false;
        }

        private void TestResultDetailsReady(TestResultDetails resultDetails)
        {
            string id = IdGenerator.Create(resultDetails.Module, resultDetails.Name);

            var detailsElement = new XElement("div", new XAttribute("class", "details"),
                                              CreateDetailsBody(resultDetails.Text));

            _browserWriter.AppendToElementId(id, detailsElement);
        }

        private static IEnumerable<XElement> CreateDetailsBody(string text)
        {
            foreach (string line in text.Split('\n', '\r'))
            {
                int leading = 0;

                while (leading < line.Length && Char.IsWhiteSpace(line[leading]))
                    leading++;

                yield return new XElement("p", 
                    new XAttribute("style", String.Format("margin-left: {0}ex", leading)),
                    new XText(line.TrimStart()));
            }
        }
    }
}
using System;
using System.Diagnostics;
using PyTestMon.Core.Config;

namespace PyTestMon.Core
{
    public class TestRunner
    {
        private readonly TestResultParser _testResultParser;

        public event Action Starting = () => { };
        public event Action Finished = () => { };
        public event Action<TestResult> TestResultReady = x => { };
        public event Action<TestResultDetails> TestResultDetailsReady = x => { };

        public TestRunner(TestResultParser testResultParser)
        {
            _testResultParser = testResultParser;
        }

        public void RunTests(SetupData setupData)
        {
            Starting();

            var args = String.Format("-B -u -m unittest discover -v {0} {1}", setupData.TestSubFolder, setupData.TestFileSpec);

            var startInfo = new ProcessStartInfo
                            {
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true,
                                FileName = setupData.PythonPath,
                                Arguments = args,
                                WorkingDirectory = setupData.ProjectFolder
                            };

            Debug.WriteLine(startInfo.ToString());

            using (var python = new Process { StartInfo = startInfo })
            {
                python.Start();

                _testResultParser.Parse(python.StandardError, TestResultReady, TestResultDetailsReady);
            }

            Finished();
        }

    }
}
using System;
using System.Configuration;
using System.Windows.Forms;
using PyTestMon.Core;
using PyTestMon.Core.Config;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace PyTestMon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            var settings = Properties.Settings.Default;

            var setupsSection = (SetupsSection)ConfigurationManager.GetSection("pytestmon");
            var browserWriter = new BrowserWriter();
            var testResultParser = new TestResultParser();
            var testRunner = new TestRunner(testResultParser);
            var taskbarManager = TaskbarManager.Instance;
            var taskbarIcons = new TaskbarIcons();
            var testFolderMonitor = new TestFolderMonitor(browserWriter, testRunner);

            var form = new PyTestMonForm(setupsSection.SetupData, browserWriter, testFolderMonitor, settings.LastSetup, taskbarManager, taskbarIcons);

            Application.Run(form);

            testFolderMonitor.Dispose();

            settings.LastSetup = form.LastSetupName;
            settings.Save();
        }
    }
}

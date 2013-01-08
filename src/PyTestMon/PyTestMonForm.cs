using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PyTestMon.Core;
using PyTestMon.Core.Config;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace PyTestMon
{
    public partial class PyTestMonForm : Form
    {
        private readonly TestFolderMonitor _testFolderMonitor;
        private readonly TaskbarManager _taskbarManager;
        private readonly TaskbarIcons _taskbarIcons;

        public string LastSetupName { get; set; }

        public PyTestMonForm(IEnumerable<SetupData> setupDataCollection, BrowserWriter browserWriter, TestFolderMonitor testFolderMonitor, string lastSetupName, TaskbarManager taskbarManager, TaskbarIcons taskbarIcons)
        {
            InitializeComponent();

            Setups.Items.AddRange(setupDataCollection.ToArray());

            browserWriter.Browser = Browser;

            _testFolderMonitor = testFolderMonitor;
            _taskbarManager = taskbarManager;
            _taskbarIcons = taskbarIcons;

            SetupData lastSetup = setupDataCollection.Where(x => x.Name == lastSetupName).SingleOrDefault();

            Setups.SelectedItem = lastSetup;

            _testFolderMonitor.TestRunComplete += TestRunComplete;

            SetControlState();
        }

        private void SetControlState()
        {
            btnStart.Enabled = Setups.SelectedItem != null && !_testFolderMonitor.Running;
            btnStop.Enabled = Setups.SelectedItem != null && _testFolderMonitor.Running;
            Setups.Enabled = !_testFolderMonitor.Running;
        }

        private void SetupsSelectedIndexChanged(object sender, EventArgs e)
        {
            var setupData = (SetupData)Setups.SelectedItem;
            _testFolderMonitor.ChangeSetup(setupData);

            SetControlState();

            LastSetupName = setupData.Name;
        }

        private void StartClick(object sender, EventArgs e)
        {
            _testFolderMonitor.Start();

            SetControlState();
        }

        private void StopClick(object sender, EventArgs e)
        {
            _testFolderMonitor.Stop();

            _taskbarManager.SetOverlayIcon(Handle, null, null);

            SetControlState();
        }

        private void TestRunComplete(bool success)
        {
            Action<bool> action = s =>
                                  {
                                      if (s)
                                          _taskbarManager.SetOverlayIcon(Handle, _taskbarIcons.OK, "OK");
                                      else
                                          _taskbarManager.SetOverlayIcon(Handle, _taskbarIcons.Fail, "Fail");
                                  };


            if (InvokeRequired)
                Invoke(action, new object[]{success});
            else
                action.Invoke(success);
        }

    }
}

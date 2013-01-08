using System;
using System.Configuration;

namespace PyTestMon.Core.Config
{
    public class SetupData : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("python-path")]
        public string PythonPath
        {
            get { return (string)this["python-path"]; }
            set { this["python-path"] = value; }
        }

        [ConfigurationProperty("project-folder")]
        public string ProjectFolder
        {
            get { return (string)this["project-folder"]; }
            set { this["project-folder"] = value; }
        }

        [ConfigurationProperty("test-sub-folder")]
        public string TestSubFolder
        {
            get { return (string)this["test-sub-folder"]; }
            set { this["test-sub-folder"] = value; }
        }

        [ConfigurationProperty("test-file-spec")]
        public string TestFileSpec
        {
            get { return (string)this["test-file-spec"]; }
            set { this["file-spec"] = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
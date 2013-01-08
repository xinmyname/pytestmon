using System.Drawing;
using System.IO;

namespace PyTestMon.Core
{
    public class TaskbarIcons
    {
        public Icon Fail { get; private set; }
        public Icon OK { get; private set; }

        public TaskbarIcons()
        {
            using (Stream str = GetResource("PyTestMon.Core.Resources.Fail-Overlay.ico"))
                Fail = new Icon(str);

            using (Stream str = GetResource("PyTestMon.Core.Resources.OK-Overlay.ico"))
                OK = new Icon(str);
        }

        private Stream GetResource(string name)
        {
            return GetType().Assembly.GetManifestResourceStream(name);
        }
    }
}
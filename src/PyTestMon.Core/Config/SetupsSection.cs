using System.Configuration;

namespace PyTestMon.Core.Config
{
    public class SetupsSection : ConfigurationSection
    {
        [ConfigurationProperty("setups", IsDefaultCollection = true)]
        public SetupDataCollection SetupData
        {
            get { return (SetupDataCollection)base["setups"]; }
        }
    }
}
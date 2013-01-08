namespace PyTestMon.Core.Config
{
    public class SetupDataCollection : ConfigurationElementCollection<SetupData>
    {
        public SetupDataCollection()
            : base("setup", e => e.Name)
        {
        }
    }
}
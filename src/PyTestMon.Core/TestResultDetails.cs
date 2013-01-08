namespace PyTestMon.Core
{
    public class TestResultDetails
    {
        public string Module { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public TestResultDetails()
        {
        }

        public TestResultDetails(string module, string name, string text)
        {
            Module = module;
            Name = name;
            Text = text;
        }
    }
}
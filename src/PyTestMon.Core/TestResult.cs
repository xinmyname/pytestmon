namespace PyTestMon.Core
{
    public class TestResult
    {
        public string Module { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public TestResult()
        {
        }

        public TestResult(string module, string name, string status)
        {
            Module = module;
            Name = name;
            Status = status;
        }
    }
}
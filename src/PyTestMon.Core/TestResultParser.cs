using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PyTestMon.Core
{
    public class TestResultParser
    {
        private readonly Regex _resultParser = new Regex(@"(?<name>[A-Za-z0-9_]*)\s\((?<module>[A-Za-z0-9_\.]*)\)\s\.\.\.\s(?<result>[A-Za-z]*)", RegexOptions.Compiled);
        private readonly Regex _failedDetailParser = new Regex(@"[A-Za-z]*:\s(?<name>[A-Za-z0-9_]*)\s\((?<module>[A-Za-z0-9_\.]*)", RegexOptions.Compiled);
        
        public void Parse(TextReader reader, Action<TestResult> resultHandler, Action<TestResultDetails> resultDetailsHandler)
        {
            string line;

            // State 1 - results
            while (!String.IsNullOrEmpty((line = reader.ReadLine())))
                ParseResult(line, resultHandler);

            // State 2 - result details
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("="))
                    ParseResultDetails(reader, resultDetailsHandler);
            }
        }

        private void ParseResult(string line, Action<TestResult> resultHandler)
        {
            Match match = _resultParser.Match(line);

            if (match.Success)
            {
                Group name = match.Groups["name"];
                Group module = match.Groups["module"];
                Group result = match.Groups["result"];
                resultHandler(new TestResult(module.Value, name.Value, result.Value));
            }
        }

        private void ParseResultDetails(TextReader reader, Action<TestResultDetails> resultDetailsHandler)
        {
            // Read test information
            string failedResult = reader.ReadLine();

            if (failedResult == null)
                return;

            Match match = _failedDetailParser.Match(failedResult);
            Group name = match.Groups["name"];
            Group module = match.Groups["module"];

            // Skip dotted line
            reader.ReadLine();

            string line;
            var text = new StringBuilder();

            // Get trace information
            while (!String.IsNullOrEmpty((line = reader.ReadLine())))
                text.AppendFormat("{0}\n", line);

            resultDetailsHandler(new TestResultDetails(module.Value, name.Value, text.ToString()));
        }
    }
}

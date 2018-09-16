using System;
using System.Windows.Documents;

namespace Demian.Client.Common
{
    public class ConsoleWindowLog : ILog
    {
        private readonly ConsoleWindow _console;

        public ConsoleWindowLog(ConsoleWindow console)
        {
            _console = console;
        }

        public void Info(string message)
        {
            _console.Add(new Run($"[ {DateTime.Now} ] {message}"));
            _console.Add(new LineBreak());
        }
    }
}
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace boticario.Logging
{
    public class AppLogger : ILogger
    {
        private readonly string loggerName;
        private readonly AppLoggerProviderConfiguration loggerConfig;

        public AppLogger(string loggerName, AppLoggerProviderConfiguration loggerConfig)
        {
            this.loggerName = loggerName;
            this.loggerConfig = loggerConfig;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"[{DateTime.Now}] - {logLevel}: [{eventId.Id}] - {formatter(state, exception)}";

            WriteInFile(message);
        }

        private void WriteInFile(string message)
        {
            string[] solutionName = Assembly.GetExecutingAssembly().GetName().Name.Split(".");
            string fileName = $"{DateTime.Today:yyyy-MM-dd}-LOG-{solutionName[0]}.log";
            string path = $@"{Directory.GetCurrentDirectory()}\LogsFiles";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, fileName);

            using (StreamWriter stream = new StreamWriter(filePath, true))
            {
                stream.WriteLine(message);
                stream.Close();
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace boticario.Logging
{
    public class AppLoggerProvider : ILoggerProvider
    {
        private readonly AppLoggerProviderConfiguration loggerConfig;
        private readonly ConcurrentDictionary<string, AppLogger> loggers = new ConcurrentDictionary<string, AppLogger>();

        public AppLoggerProvider(AppLoggerProviderConfiguration loggerConfig)
        {
            this.loggerConfig = loggerConfig;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new AppLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

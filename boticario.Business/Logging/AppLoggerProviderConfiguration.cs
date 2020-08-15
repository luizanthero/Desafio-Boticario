using Microsoft.Extensions.Logging;

namespace boticario.Logging
{
    public class AppLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public int EventId { get; set; } = 0;
    }
}

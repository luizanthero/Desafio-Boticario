﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

namespace boticario.Logging
{
    public class AppLoggerProvider : ILoggerProvider
    {
        private readonly AppLoggerProviderConfiguration loggerConfig;
        private readonly ConcurrentDictionary<string, AppLogger> loggers = new ConcurrentDictionary<string, AppLogger>();

        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public AppLoggerProvider(AppLoggerProviderConfiguration loggerConfig)
        {
            this.loggerConfig = loggerConfig;
        }

        public AppLoggerProvider(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<AppLoggerProvider>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            finally
            {
                logger.LogInformation("Request {method} | URL: {url} | StatusCode: {statusCode}", 
                    context.Request?.Method, context.Request?.Path.Value, context.Response?.StatusCode);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new AppLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            return;
        }
    }
}

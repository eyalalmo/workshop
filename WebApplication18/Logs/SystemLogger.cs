using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace WebApplication18.Logs
{
    class SystemLogger
    {
        private static readonly ILog eventLog = LogManager.GetLogger("Event Log");
        private static readonly ILog errorLog = LogManager.GetLogger("Error Log");
        public static void ConfigureFileAppender(string eventLog , string errorLog)
        {
            IAppender[] fileAppender = { GetFileAppender(eventLog), GetFileAppender(errorLog) };
            BasicConfigurator.Configure(fileAppender);
            ((Hierarchy)LogManager.GetRepository()).Root.Level = Level.All;
        }

        public static ILog getEventLog()
        {
            return eventLog;
        }

        public static ILog getErrorLog()
        {
            return errorLog;
        }

        private static IAppender GetFileAppender(string logFile)
        {
            var layout = new PatternLayout("%date %-5level %logger - %message%newline");
            layout.ActivateOptions(); // According to the docs this must be called as soon as any properties have been changed.

            var appender = new FileAppender
            {
                File = logFile,
                Encoding = Encoding.UTF8,
                Threshold = Level.All,
                Layout = layout
            };

            appender.ActivateOptions();

            return appender;
        }
    }
}

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
        public static void configureLogs()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Threshold = Level.Debug;

            // Configure LoggerA
            string logNameA = @"EventLog";
            string fileNameA = @"Event_Log.txt";
            var loggerA = hierarchy.LoggerFactory.CreateLogger(LogManager.GetRepository(),"EventLog");
            loggerA.Hierarchy = hierarchy;
            loggerA.AddAppender(CreateFileAppender(logNameA, fileNameA));
            loggerA.Repository.Configured = true;
            loggerA.Level = Level.Debug;

            eventLog = new LogImpl(loggerA);

            // Configure LoggerB

            string logNameB = @"ErrorLog";
            string fileNameB = @"Error_Log.txt";
            var loggerB = hierarchy.LoggerFactory.CreateLogger(LogManager.GetRepository(), "ErrorLog");
            loggerB.Hierarchy = hierarchy;
            loggerB.AddAppender(CreateFileAppender(logNameB, fileNameB));
            loggerB.Repository.Configured = true;
            loggerB.Level = Level.Debug;

             errorLog = new LogImpl(loggerB);
        }
        private static ILog eventLog;
        private static ILog errorLog;

        private static IAppender CreateFileAppender(string name, string fileName)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date %level %logger: %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = 2;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaximumFileSize = "10MB";
            appender.Layout = patternLayout;
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.StaticLogFileName = true;
            appender.ActivateOptions();
            return appender;
        }

        public static ILog getEventLog()
        {
            return eventLog;
        }

        public static ILog getErrorLog()
        {
            return errorLog;
        }

        //private static IAppender GetFileAppender(string logFile)
        //{
        //    var layout = new PatternLayout("%date %-5level %logger - %message%newline");
        //    layout.ActivateOptions(); // According to the docs this must be called as soon as any properties have been changed.

        //    var appender = new FileAppender
        //    {
        //        File = logFile,
        //        Encoding = Encoding.UTF8,
        //        Threshold = Level.All,
        //        Layout = layout
        //    };

        //    appender.ActivateOptions();

        //    return appender;
        //}
    }
}

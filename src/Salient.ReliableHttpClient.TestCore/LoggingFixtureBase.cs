﻿using System.Text;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.TestCore
{
    public class LoggingFixtureBase
    {
        private static readonly StringBuilder LogOutput = new StringBuilder();

        static LoggingFixtureBase()
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                                           =>
                                               {
                                                   var logger = new SimpleDebugAppender(logName, logLevel, showLevel,
                                                                                        showDateTime, showLogName,
                                                                                        dateTimeFormat);
                                                   logger.LogEvent += (s, e) =>
                                                                          {
                                                                              lock (LogOutput)
                                                                              {
                                                                                  LogOutput.AppendLine(
                                                                                      string.Format("{0} {1} {2}",
                                                                                                    e.Level, e.Message,
                                                                                                    e.Exception));
                                                                              }
                                                                          };
                                                   return logger;
                                               };
        }

        public static string GetLogOutput()
        {
            lock (LogOutput)
            {
                string output = LogOutput.ToString();
                LogOutput.Clear();
                return output;
            }
        }
    }
}
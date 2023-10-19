using NLog;
using NLog.Config;
using NLog.Targets;

namespace Masterclass.Revit.Utilities
{
    public static class NLogUtils
    {
        public static LoggingConfiguration Config { get; set; } = CreateConfiguration();

        public static LoggingConfiguration CreateConfiguration()
        {
            var config = new LoggingConfiguration();

            // (Konrad) File Log Setup
            var fileTarget = new FileTarget
            {
                Name = "default",
                FileName = @"${specialfolder:folder=ApplicationData}/Masterclass/logs/Debug.log",
                Layout = @"${longdate}|${level:uppercase=true}|${logger}|${message}|${event-properties:item=User}|${exception:format=ToString}",
                KeepFileOpen = false,
                ArchiveFileName = @"${specialfolder:folder=ApplicationData}/Masterclass/logs/Debug_${shortdate}.{##}.log",
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 30
            };
            config.AddTarget("logfile", fileTarget);

            var rule1 = new LoggingRule("masterclass_logger", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule1);

            return config;
        }

        public static Logger GetMasterclassLogger()
        {
            LogManager.Configuration = Config;
            return LogManager.GetLogger("masterclass_logger");
        }
    }
}

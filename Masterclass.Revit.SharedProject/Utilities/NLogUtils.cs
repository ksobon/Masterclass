using Elastic.CommonSchema.NLog;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.ElasticSearch;

namespace Masterclass.Revit.Utilities
{
    public static class NLogUtils
    {
        public static LoggingConfiguration Config { get; set; } = CreateConfiguration();

        public static LoggingConfiguration CreateConfiguration()
        {
            var config = new LoggingConfiguration();

            // (Konrad) File Setup
            var fileTarget = new FileTarget
            {
                Name = "default",
                FileName = @"${specialfolder:folder=ApplicationData}/Masterclass/logs/Debug.log",
                Layout = @"${longdate}|${level:uppercase=true}|${logger}|${message}|${event-properties:item=Application}|${event-properties:item=Version}|${exception:format=ToString}",
                KeepFileOpen = false,
                ArchiveFileName = @"${specialfolder:folder=ApplicationData}/Masterclass/logs/Debug_${shortdate}.{##}.log",
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 30
            };

            // (Konrad) ElasticSearch Setup
            Layout.Register<EcsLayout>("EcsLayout");

            var esTarget = new ElasticSearchTarget
            {
                CloudId = "nlog-test:dXMtZWFzdC0yLmF3cy5lbGFzdGljLWNsb3VkLmNvbTo0NDMkMWU4Y2U5ODIxMzc1NDQxOGEzYTIzZThjZDA4MzU3OWQkZTE4MGFjZTAxZjQyNGI2NDk0MGIzMjdkN2Q1ODE4NmY=",
                Username = "elastic",
                Password = "s2vCsd5i3mbKvhyBTfgpA6aC",
                Index = "masterclass-logs",
                DocumentType = "",
                IncludeDefaultFields = true,
                IncludeAllProperties = true,
                Layout = new EcsLayout(),
                EnableJsonLayout = true
            };

            config.AddTarget("logfile", fileTarget);
            config.AddTarget("es_database", esTarget);

            var rule1 = new LoggingRule("masterclass_logger", LogLevel.Trace, fileTarget);
            rule1.Targets.Add(esTarget);
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

using NLog;

namespace Masterclass.Revit.Utilities
{
    public static class NLogManager
    {
        public static ILogger Logger = NLogUtils.GetMasterclassLogger();

        public static void InfoLog(string msg)
        {
            var info = new LogEventInfo(LogLevel.Info, "", msg);
            info.SetGlobalParameters();
            Logger.Info(info);
        }

        public static void ErrorLog(string msg)
        {
            var info = new LogEventInfo(LogLevel.Error, "", msg);
            info.SetGlobalParameters();
            Logger.Error(info);
        }

        public static void SetGlobalParameters(this LogEventInfo info)
        {
            info.Properties.Add("Application", "Masterclass App");
            info.Properties.Add("Version", typeof(NLogManager).Assembly.GetName().Version);
        }
    }
}

namespace MiFramework
{
    public enum DebugLevel
    {
        None,
        Exception,
        Error,
        Warning,
        Normal
    }

    public class Debug
    {
        public static DebugLevel staticLevel = DebugLevel.Normal;
        private static readonly IDebugger logger = new FileLog();
        public static void Log(string msg)
        {
            if (staticLevel >= DebugLevel.Normal)
                logger.Print(msg);
        }
        public static void LogWarning(string msg)
        {
            if (staticLevel >= DebugLevel.Warning)
                logger.PrintWarning(msg);
        }
        public static void LogError(string msg)
        {
            if (staticLevel >= DebugLevel.Error)
                logger.PrintError(msg);
        }
        public static void LogException(string msg)
        {
            if (staticLevel >= DebugLevel.Exception)
                logger.PrintException(msg);
        }

        private readonly IDebugger debugger;
        public DebugLevel level;

        public Debug(IDebugger debugger, DebugLevel level)
        {
            this.debugger = debugger;
            this.level = level;
        }

        public void Print(string msg)
        {
            if (staticLevel >= DebugLevel.Normal)
                debugger.Print(msg);
        }
        public void PrintWarning(string msg)
        {
            if (staticLevel >= DebugLevel.Warning)
                debugger.PrintWarning(msg);
        }
        public void PrintError(string msg)
        {
            if (staticLevel >= DebugLevel.Error)
                debugger.PrintError(msg);
        }
        public void PrintException(string msg)
        {
            if (staticLevel >= DebugLevel.Exception)
                debugger.PrintException(msg);
        }
    }
}

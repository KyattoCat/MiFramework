namespace MiFramework
{
    public interface IDebugger
    {
        void Print(string msg);
        void PrintWarning(string msg);
        void PrintError(string msg);
        void PrintException(string msg);
    }
}
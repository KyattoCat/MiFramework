using System.Diagnostics;
using System.Text;

namespace MiFramework
{
    public class FileLog : IDebugger
    {
        private FileStream? stream;
        public FileLog()
        {
            string logDir = ".\\log";
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string path = $"{logDir}\\log_{timestamp}.txt";
            stream = File.Open(path, FileMode.Append);
        }

        public FileLog(string logDir)
        {
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string path = $"{logDir}\\log_{timestamp}.txt";
            stream = File.Open(path, FileMode.Append);
        }

        private StringBuilder GenerateHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.UtcNow.ToString());
            sb.Append(": ");
            return sb;
        }

        private void Flush(StringBuilder sb)
        {
            if (stream != null)
            {
                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }

        public void Print(string msg)
        {
            Flush(
                GenerateHeader()
                .Append(msg)
            );
        }

        public void PrintWarning(string msg)
        {
            Flush(
                GenerateHeader()
                .AppendLine("[WARNING]:")
                .Append(msg)
            );
        }

        public void PrintError(string msg)
        {
            Flush(
                GenerateHeader()
                .AppendLine("[ERROR]:")
                .AppendLine(msg)
                .AppendLine("[STACKTRACE]:")
                .Append(new StackTrace().ToString())
            );
        }

        public void PrintException(string msg)
        {
            Flush(
                GenerateHeader()
                .AppendLine("[EXCEPTION]:")
                .AppendLine(msg)
                .AppendLine("[STACKTRACE]:")
                .Append(new StackTrace().ToString())
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public static class Logger
    {
        private static readonly string logFilePath = "app.log";

        static Logger()
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogError(string message)
        {
            Log("ERROR", message);
        }

        public static void LogException(Exception ex)
        {
            var message = $"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}";
            LogInfo(ex.Message);
            LogError(message);
        }

        private static void Log(string logLevel, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}

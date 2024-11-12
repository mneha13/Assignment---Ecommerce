using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public static class Logger
    {
        static Logger()
        {
            if (!File.Exists(StaticData.logFilePath))
            {
                File.Create(StaticData.logFilePath).Dispose();
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
            File.AppendAllText(StaticData.logFilePath, logEntry + Environment.NewLine);
        }
    }
}

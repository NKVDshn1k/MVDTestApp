using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVDTestApp.Logging
{
    class FileLogger : ILogger
    {
        private const string _logFilePath = "Erors.log";
        public void Info(string message, string infoType,object caller, [CallerMemberName] string methood = "unknown")
        {
            using (StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine($"{DateTime.UtcNow.ToString()} | {infoType} | {caller} | {methood} | {message}");
            }
        }

        public async Task InfoAsync(string message, string infoType, object caller, [CallerMemberName] string methood = "unknown")
        {
            using (StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                await writer.WriteLineAsync($"{DateTime.UtcNow.ToString()} | {infoType} | {caller} | {methood} | {message}");
            }
        }
    }
}

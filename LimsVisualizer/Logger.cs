using System;
using System.IO;

namespace LimsVisualizer
{
    public class Logger
    {
        private StreamWriter mLogWriter;

        public string LogFilePath { get; private set; }

        public void Initialize()
        {
// ReSharper disable AssignNullToNotNullAttribute
            LogFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "LimsVisualizer");
// ReSharper restore AssignNullToNotNullAttribute
            var file = Path.Combine(LogFilePath, string.Format("{0:yyyyMMdd_HHmmss}.txt", DateTime.Now));

            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);                
            }

            var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read);
            mLogWriter = new StreamWriter(fileStream);
            mLogWriter.WriteLine("[START] [{0:dd.MM.yyyy HH:mm:ss}.{1}] Limsvisualizer was started. Version: {2}",
                DateTime.Now,
                DateTime.Now.Millisecond,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            mLogWriter.Flush();
        }

        public void WriteDebugMessage(string message)
        {
            mLogWriter.WriteLine("[DEBUG] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message);
            mLogWriter.Flush();
        }

        public void WriteFailureMessage(string message)
        {
            mLogWriter.WriteLine("[FAILURE] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message);
            mLogWriter.Flush();
        }

        public void WriteException(Exception exception)
        {
            mLogWriter.WriteLine("[EXCEPTION] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, exception);
            mLogWriter.Flush();
        }

        public void Dispose()
        {
            mLogWriter.WriteLine("[SHUTDOWN] [{0:dd.MM.yyyy HH:mm:ss}.{1}] Limsvisualizer was shut down.", DateTime.Now, DateTime.Now.Millisecond);
            mLogWriter.Flush();
            mLogWriter.Close();
        }
    }
}

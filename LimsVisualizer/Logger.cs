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
            LogFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "LimsVisualizer");
            var file = Path.Combine(LogFilePath, string.Format("{0:yyyyMMdd_HHmmss}.txt", DateTime.Now));

            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);                
            }

            var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read);
            mLogWriter = new StreamWriter(fileStream);
            mLogWriter.WriteLine(string.Format("[START] [{0:dd.MM.yyyy HH:mm:ss}.{1}] Limsvisualizer was started. Version: {2}",
                DateTime.Now,
                DateTime.Now.Millisecond,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));
            mLogWriter.Flush();
        }

        public void WriteDebugMessage(string message)
        {
            mLogWriter.WriteLine(string.Format("[DEBUG] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message));
            mLogWriter.Flush();
        }

        public void WriteFailureMessage(string message)
        {
            mLogWriter.WriteLine(string.Format("[FAILURE] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message));
            mLogWriter.Flush();
        }

        public void WriteException(Exception exception)
        {
            mLogWriter.WriteLine(string.Format("[EXCEPTION] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, exception));
            mLogWriter.Flush();
        }

        public void Dispose()
        {
            mLogWriter.WriteLine(string.Format("[SHUTDOWN] [{0:dd.MM.yyyy HH:mm:ss}.{1}] Limsvisualizer was shut down.", DateTime.Now, DateTime.Now.Millisecond));
            mLogWriter.Flush();
            mLogWriter.Close();
        }
    }
}

using System;
using System.IO;

namespace LimsHelper
{
    public class Logger
    {
        private StreamWriter mLogWriter;

        public string LogFilePath { private get; set; }
        public string ApplicationName { private get; set; }
        public string ApplicationVersion { private get; set; }

        public void Initialize()
        {
            var file = Path.Combine(LogFilePath, string.Format("{0:yyyyMMdd_HHmmss}.txt", DateTime.Now));

            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);                
            }

            var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read);
            mLogWriter = new StreamWriter(fileStream);
            mLogWriter.WriteLine("[START] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2} was started. Version: {3}",
                DateTime.Now,
                DateTime.Now.Millisecond,
                ApplicationName,
                ApplicationVersion);
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
            mLogWriter.WriteLine("[SHUTDOWN] [{0:dd.MM.yyyy HH:mm:ss}.{1}] {2} was shut down.", DateTime.Now, DateTime.Now.Millisecond, ApplicationName);
            mLogWriter.Flush();
            mLogWriter.Close();
        }
    }
}

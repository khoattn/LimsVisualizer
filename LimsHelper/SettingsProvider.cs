using System;
using System.Globalization;
using System.IO;

namespace LimsHelper
{
    public class SettingsProvider
    {
        public string ApplicationName { private get; set; }
        public Logger LogWriter { private get; set; }

        public void GetSettings(out LimsVisualizerSettings limsVisualizerSettings)
        {
            try
            {
                limsVisualizerSettings = new LimsVisualizerSettings();
                var settingsFile = _GetSettingsFilePath();

                LogWriter.WriteDebugMessage(string.Format("Reading settings. File: '{0}'", settingsFile));
                if (!File.Exists(settingsFile))
                {
                    limsVisualizerSettings.FilePath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\Anton Paar\Davis 5\Data Monitoring\";
                    limsVisualizerSettings.DueTime = new TimeSpan(0, 0, 0, 0, 500);
                    LogWriter.WriteDebugMessage("Settings file not found, using default settings.");
                    return;
                }

                var fileStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var settingsReader = new StreamReader(fileStream);

                limsVisualizerSettings.FilePath = settingsReader.ReadLine();
                limsVisualizerSettings.DueTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(settingsReader.ReadLine()));
                settingsReader.Close();
                LogWriter.WriteDebugMessage(string.Format("Settings file found. FilePath: '{0}' DueTime: '{1}'",
                    limsVisualizerSettings.FilePath,
                    limsVisualizerSettings.DueTime));
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Reading settings file failed!");
                LogWriter.WriteException(exception);
                throw;
            }
        }

        public void GetSettings(out LimsSimulatorSettings limsSimulatorSettings)
        {
            try
            {
                limsSimulatorSettings = new LimsSimulatorSettings();
                var settingsFile = _GetSettingsFilePath();

                LogWriter.WriteDebugMessage(string.Format("Reading settings. File: '{0}'", settingsFile));
                if (!File.Exists(settingsFile))
                {
                    limsSimulatorSettings.SampleFile = string.Empty;
                    limsSimulatorSettings.DestinationPath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\Anton Paar\Davis 5\Data Monitoring\";
                    limsSimulatorSettings.DueTime = new TimeSpan(0, 0, 0, 1);
                    LogWriter.WriteDebugMessage("Settings file not found, using default settings.");
                    return;
                }

                var fileStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var settingsReader = new StreamReader(fileStream);

                limsSimulatorSettings.SampleFile = settingsReader.ReadLine();
                limsSimulatorSettings.DestinationPath = settingsReader.ReadLine();
                limsSimulatorSettings.DueTime = new TimeSpan(0, 0, Convert.ToInt16(settingsReader.ReadLine()));
                settingsReader.Close();
                LogWriter.WriteDebugMessage(string.Format("Settings file found. SampleFile: '{0}' DestinationPath: '{1}' DueTime: '{2}'",
                        limsSimulatorSettings.SampleFile,
                        limsSimulatorSettings.DestinationPath,
                        limsSimulatorSettings.DueTime));
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Reading settings file failed!");
                LogWriter.WriteException(exception);
                throw;
            }
        }

        public void WriteSettings(LimsVisualizerSettings limsVisualizerSettings)
        {
            try
            {
                var settingsFile = _GetSettingsFilePath();

                LogWriter.WriteDebugMessage(string.Format("Writing settings file. FilePath: '{0}' DueTime: '{1}'",
                    limsVisualizerSettings.FilePath,
                    limsVisualizerSettings.DueTime));
                var fileStream = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.None);
                var settingsWriter = new StreamWriter(fileStream);

                settingsWriter.WriteLine(limsVisualizerSettings.FilePath);
                settingsWriter.WriteLine(limsVisualizerSettings.DueTime.TotalMilliseconds);
                settingsWriter.Close();
                LogWriter.WriteDebugMessage("Settings file was successfully written.");
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Writing settings file failed!");
                LogWriter.WriteException(exception);
                throw;
            }
        }

        public void WriteSettings(LimsSimulatorSettings limsSimulatorSettings)
        {
            try
            {
                var settingsFile = _GetSettingsFilePath();

                LogWriter.WriteDebugMessage(
                    string.Format("Writing settings file. SampleFile: '{0}' DestinationPath: '{1}' DueTime: '{2}'",
                        limsSimulatorSettings.SampleFile,
                        limsSimulatorSettings.DestinationPath,
                        limsSimulatorSettings.DueTime));
                var fileStream = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.None);
                var settingsWriter = new StreamWriter(fileStream);

                settingsWriter.WriteLine(limsSimulatorSettings.SampleFile);
                settingsWriter.WriteLine(limsSimulatorSettings.DestinationPath);
                settingsWriter.WriteLine(limsSimulatorSettings.DueTime.TotalSeconds);
                settingsWriter.Close();
                LogWriter.WriteDebugMessage("Settings file was successfully written.");
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Writing settings file failed!");
                LogWriter.WriteException(exception);
                throw;
            }
        }

        private string _GetSettingsFilePath()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("TEMP"), ApplicationName, @"settings.txt");
        }
    }

    public class LimsVisualizerSettings
    {
        public string FilePath { get; set; }
        public TimeSpan DueTime { get; set; }
    }

    public class LimsSimulatorSettings
    {
        public string SampleFile { get; set; }
        public string DestinationPath { get; set; }
        public TimeSpan DueTime { get; set; }
    }
}

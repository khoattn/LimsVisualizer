using System;
using System.Globalization;
using System.IO;
using LimsHelper;

namespace LimsVisualizer
{
    class SettingsProvider
    {
        private string SettingsFile
        {
            get { return Path.Combine(Environment.GetEnvironmentVariable("TEMP"), ApplicationName ,@"settings.txt"); }
        }

        public string ApplicationName { private get; set; }
        public Logger LogWriter { private get; set; }

        public void GetSettings(out LimsVisualizerSettings limsVisualizerSettings)
        {
            try
            {
                limsVisualizerSettings = new LimsVisualizerSettings();

                LogWriter.WriteDebugMessage(string.Format("Reading settings. File: '{0}'", SettingsFile));
                if (!File.Exists(SettingsFile))
                {
                    limsVisualizerSettings.FilePath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\Anton Paar\Davis 5\Data Monitoring\";
                    limsVisualizerSettings.DueTime = 500;
                    LogWriter.WriteDebugMessage("Settings file not found, using default settings.");
                    return;
                }

                var fileStream = new FileStream(SettingsFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var settingsReader = new StreamReader(fileStream);

                limsVisualizerSettings.FilePath = settingsReader.ReadLine();
                limsVisualizerSettings.DueTime = Convert.ToInt16(settingsReader.ReadLine());
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

                LogWriter.WriteDebugMessage(string.Format("Reading settings. File: '{0}'", SettingsFile));
                if (!File.Exists(SettingsFile))
                {
                    limsSimulatorSettings.SampleFile = string.Empty;
                    limsSimulatorSettings.DestinationPath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\Anton Paar\Davis 5\Data Monitoring\";
                    limsSimulatorSettings.DueTime = 500;
                    LogWriter.WriteDebugMessage("Settings file not found, using default settings.");
                    return;
                }

                var fileStream = new FileStream(SettingsFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var settingsReader = new StreamReader(fileStream);

                limsSimulatorSettings.SampleFile = settingsReader.ReadLine();
                limsSimulatorSettings.DestinationPath = settingsReader.ReadLine();
                limsSimulatorSettings.DueTime = Convert.ToInt16(settingsReader.ReadLine());
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
                LogWriter.WriteDebugMessage(string.Format("Writing settings file. FilePath: '{0}' DueTime: '{1}'",
                    limsVisualizerSettings.FilePath,
                    limsVisualizerSettings.DueTime));
                var fileStream = new FileStream(SettingsFile, FileMode.Create, FileAccess.Write, FileShare.None);
                var settingsWriter = new StreamWriter(fileStream);

                settingsWriter.WriteLine(limsVisualizerSettings.FilePath);
                settingsWriter.WriteLine(limsVisualizerSettings.DueTime.ToString(CultureInfo.InvariantCulture));
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
                LogWriter.WriteDebugMessage(
                    string.Format("Writing settings file. SampleFile: '{0}' DestinationPath: '{1}' DueTime: '{2}'",
                        limsSimulatorSettings.SampleFile,
                        limsSimulatorSettings.DestinationPath,
                        limsSimulatorSettings.DueTime));
                var fileStream = new FileStream(SettingsFile, FileMode.Create, FileAccess.Write, FileShare.None);
                var settingsWriter = new StreamWriter(fileStream);

                settingsWriter.WriteLine(limsSimulatorSettings.SampleFile);
                settingsWriter.WriteLine(limsSimulatorSettings.DestinationPath);
                settingsWriter.WriteLine(limsSimulatorSettings.DueTime.ToString(CultureInfo.InvariantCulture));
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
    }

    class LimsVisualizerSettings
    {
        public string FilePath { get; set; }
        public int DueTime { get; set; }
    }

    class LimsSimulatorSettings
    {
        public string SampleFile { get; set; }
        public string DestinationPath { get; set; }
        public int DueTime { get; set; }
    }
}

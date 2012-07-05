using System;
using System.Globalization;
using System.IO;

namespace LimsVisualizer
{
    class SettingsProvider
    {
        private readonly string mSettingsFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), @"LimsVisualizer\settings.txt");

        public void GetSettings()
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage(string.Format("Reading settings. File: '{0}'", mSettingsFile));
                if (!File.Exists(mSettingsFile))
                {
                    MainForm.Path = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\Anton Paar\Davis 5\Data Monitoring\";
                    MainForm.DueTime = 500;
                    MainForm.LogWriter.WriteDebugMessage("Settings file not found, using default settings.");
                    return;
                }

                var fileStream = new FileStream(mSettingsFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var settingsReader = new StreamReader(fileStream);

                MainForm.Path = settingsReader.ReadLine();
                MainForm.DueTime = Convert.ToInt16(settingsReader.ReadLine());
                settingsReader.Close();
                MainForm.LogWriter.WriteDebugMessage(string.Format("Settings file found. Path: '{0}' DueTime: '{1}'", MainForm.Path, MainForm.DueTime));
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Reading settings file failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        public void WriteSettings()
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage(string.Format("Writing settings file. Path: '{0}' DueTime: '{1}'", MainForm.Path, MainForm.DueTime));
                var fileStream = new FileStream(mSettingsFile, FileMode.Create, FileAccess.Write, FileShare.None);
                var settingsWriter = new StreamWriter(fileStream);

                settingsWriter.WriteLine(MainForm.Path);
                settingsWriter.WriteLine(MainForm.DueTime.ToString(CultureInfo.InvariantCulture));
                settingsWriter.Close();
                MainForm.LogWriter.WriteDebugMessage("Settings file was successfully written.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Writing settings file failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }
    }
}

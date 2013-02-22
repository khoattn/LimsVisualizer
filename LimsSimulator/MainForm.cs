using System;
using System.Globalization;
using System.IO;
using LimsHelper;
using LimsSimulator.Properties;
using Threading = System.Threading;
using System.Windows.Forms;

namespace LimsSimulator
{
    public partial class MainForm : Form
    {
        private static bool sIsHandlerDisposed = true;
        private static Threading.Timer sTimer;
        private readonly Logger mLogWriter = new Logger();
        private int mExtensionCounter;

        public static bool HeadersWritten;
        private static LimsSimulatorSettings sLimsSimulatorSettings;

        private static readonly SettingsProvider sSettingsProvider = new SettingsProvider();
        private static MainForm sInstance;

        public MainForm()
        {
            InitializeComponent();
            var application = System.Reflection.Assembly.GetExecutingAssembly().GetName();

            sInstance = this;
            mLogWriter.ApplicationName = application.Name;
            mLogWriter.ApplicationVersion = application.Version.ToString();
            mLogWriter.LogFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), application.Name);
            sSettingsProvider.ApplicationName = application.Name;
            sSettingsProvider.LogWriter = mLogWriter;
        }

        private void _FormMainLoad(object sender, EventArgs e)
        {
            var interval = new[]
            {
                "1 second",
                "2 seconds",
                "3 seconds",
                "5 seconds",
                "10 seconds",
                "15 seconds",
                "20 seconds",
                "30 seconds",
                "1 minute",
                "2 minutes",
                "3 minutes",
                "5 minutes",
                "10 minutes"
            };

            comboBoxInterval.DataSource = interval;

            mLogWriter.Initialize();

            sSettingsProvider.GetSettings(out sLimsSimulatorSettings);
            textBoxDestinationPath.Text = folderBrowserDialogDestinationPath.SelectedPath = sLimsSimulatorSettings.DestinationPath;
            textBoxSampleFile.Text = sLimsSimulatorSettings.SampleFile;

            comboBoxInterval.SelectedIndex = _GetIndexOfComboboxItem(sLimsSimulatorSettings.DueTime);
        }

        private int _GetIndexOfComboboxItem(TimeSpan timeSpan)
        {
            switch ((int)timeSpan.TotalSeconds)
            {
                case 1:
                    return comboBoxInterval.Items.IndexOf("1 second");
                case 2:
                    return comboBoxInterval.Items.IndexOf("2 seconds");
                case 3:
                    return comboBoxInterval.Items.IndexOf("3 seconds");
                case 5:
                    return comboBoxInterval.Items.IndexOf("5 seconds");
                case 10:
                    return comboBoxInterval.Items.IndexOf("10 seconds");
                case 15:
                    return comboBoxInterval.Items.IndexOf("15 seconds");
                case 20:
                    return comboBoxInterval.Items.IndexOf("20 seconds");
                case 30:
                    return comboBoxInterval.Items.IndexOf("30 seconds");
                case 60:
                    return comboBoxInterval.Items.IndexOf("1 minute");
                case 120:
                    return comboBoxInterval.Items.IndexOf("2 minutes");
                case 180:
                    return comboBoxInterval.Items.IndexOf("3 minutes");
                case 300:
                    return comboBoxInterval.Items.IndexOf("5 minutes");
                case 600:
                    return comboBoxInterval.Items.IndexOf("10 minutes");
                default:
                    throw new IndexOutOfRangeException(comboBoxInterval.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void _FormMainClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                sLimsSimulatorSettings.SampleFile = textBoxSampleFile.Text;
                sLimsSimulatorSettings.DestinationPath = textBoxDestinationPath.Text;

                sSettingsProvider.WriteSettings(sLimsSimulatorSettings);
                mLogWriter.Dispose();
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        public static void ShowErrorMessage(Exception exception)
        {
            var errorMessage = new PopupMessage { Type = PopupMessageType.Error };

            if (!sIsHandlerDisposed)
            {
                _StopHandler();
                sInstance._ChangeStateOfAllControls();
            }

            errorMessage.ShowDialog(ActiveForm, exception.Message, exception.StackTrace);
        }

        private void _ChangeStateOfAllControls()
        {
            textBoxSampleFile.Enabled = !textBoxSampleFile.Enabled;
            textBoxDestinationPath.Enabled = !textBoxDestinationPath.Enabled;
            comboBoxInterval.Enabled = !comboBoxInterval.Enabled;
            buttonBrowseDestinationPath.Enabled = !buttonBrowseDestinationPath.Enabled;
            buttonBrowseSamplePath.Enabled = !buttonBrowseSamplePath.Enabled;

            if (buttonStart.Text == Resources.Start)
                _ChangeStartButtonToStop();
            else
                _ChangeStopButtonToStart();
        }

        private void _ChangeStartButtonToStop()
        {
            buttonStart.Text = Resources.Stop;
            buttonStart.Click += _Stop;
            buttonStart.Click -= _Start;
        }

        private void _ChangeStopButtonToStart()
        {
            buttonStart.Text = Resources.Start;
            buttonStart.Click -= _Stop;
            buttonStart.Click += _Start;
        }

        private void _ButtonBrowseSamplePathClick(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialogSampleFile.ShowDialog())
            {
                textBoxSampleFile.Text = openFileDialogSampleFile.FileName;
            }
        }

        private void _ButtonBrowseDestinationPathClick(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialogDestinationPath.ShowDialog())
            {
                textBoxDestinationPath.Text = folderBrowserDialogDestinationPath.SelectedPath;
            }
        }

        private void _ComboBoxIntervalSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxInterval.SelectedIndex)
            {
                case 0:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0,0,1);
                    break;
                case 1:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 2);
                    break;
                case 2:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 3);
                    break;
                case 3:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 5);
                    break;
                case 4:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 10);
                    break;
                case 5:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 15);
                    break;
                case 6:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 20);
                    break;
                case 7:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 0, 30);
                    break;
                case 8:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 1, 0);
                    break;
                case 9:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 2, 0);
                    break;
                case 10:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 3, 0);
                    break;
                case 11:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 5, 0);
                    break;
                case 12:
                    sLimsSimulatorSettings.DueTime = new TimeSpan(0, 10, 0);
                    break;
                default:
                    throw new IndexOutOfRangeException(comboBoxInterval.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void _Start(object sender, EventArgs e)
        {
            if ((textBoxDestinationPath.Text == string.Empty) || (textBoxSampleFile.Text == string.Empty))
            {
                MessageBox.Show(Resources.EmptyTextBoxMessage, Resources.Info, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            if (!File.Exists(textBoxSampleFile.Text))
            {
                MessageBox.Show(Resources.SampleFileNotExists, Resources.Info, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            sLimsSimulatorSettings.SampleFile = textBoxSampleFile.Text;
            sLimsSimulatorSettings.DestinationPath = textBoxDestinationPath.Text;

            if (!Directory.Exists(sLimsSimulatorSettings.DestinationPath))
            {
                Directory.CreateDirectory(sLimsSimulatorSettings.DestinationPath);
            }

            sTimer = new Threading.Timer(_TimerHandler);
            _StartHandler();
            _ChangeStateOfAllControls();
        }

        private void _Stop(object sender, EventArgs e)
        {
            _StopHandler();
            _ChangeStateOfAllControls();
        }

        private void _TimerHandler(object state)
        {
            _WriteFile();

            //Only start handler if it is not disposed
            if (!sIsHandlerDisposed)
                _StartHandler();
        }

        private void _WriteFile()
        {
            File.Copy(sLimsSimulatorSettings.SampleFile, _GetNextFileName(), true);
        }

        private string _GetNextFileName()
        {
            var fileName = Path.GetFileNameWithoutExtension(sLimsSimulatorSettings.SampleFile);
            var newFileNameWithExtension = string.Format("{0}.{1}", fileName, mExtensionCounter);
            var destinationPath = Path.Combine(sLimsSimulatorSettings.DestinationPath, newFileNameWithExtension);
            mExtensionCounter++;
            return destinationPath;
        }

        private static void _StopHandler()
        {
            sIsHandlerDisposed = true;
            sTimer.Dispose();
        }

        private void _StartHandler()
        {
            sIsHandlerDisposed = false;
            sTimer.Change(sLimsSimulatorSettings.DueTime, Threading.Timeout.InfiniteTimeSpan);
        }
    }
}

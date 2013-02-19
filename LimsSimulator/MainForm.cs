using System;
using System.Globalization;
using LimsSimulator.Properties;
using Threading = System.Threading;
using System.Windows.Forms;

namespace LimsSimulator
{
    public partial class MainForm : Form
    {
        private static bool sIsHandlerDisposed;
        private static Threading.Timer sTimer;
        private TimeSpan mDueTime;
        private string mSampleFile;
        private string mDestinationPath;

        public MainForm()
        {
            InitializeComponent();
        }

        private void _LimsSimulatorLoad(object sender, EventArgs e)
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
                    mDueTime = new TimeSpan(0,0,1);
                    break;
                case 1:
                    mDueTime = new TimeSpan(0,0,2);
                    break;
                case 2:
                    mDueTime = new TimeSpan(0,0,3);
                    break;
                case 3:
                    mDueTime = new TimeSpan(0,0,5);
                    break;
                case 4:
                    mDueTime = new TimeSpan(0,0,10);
                    break;
                case 5:
                    mDueTime = new TimeSpan(0,0,15);
                    break;
                case 6:
                    mDueTime = new TimeSpan(0,0,20);
                    break;
                case 7:
                    mDueTime = new TimeSpan(0,0,30);
                    break;
                case 8:
                    mDueTime = new TimeSpan(0,1,0);
                    break;
                case 9:
                    mDueTime = new TimeSpan(0,2,0);
                    break;
                case 10:
                    mDueTime = new TimeSpan(0,3,0);
                    break;
                case 11:
                    mDueTime = new TimeSpan(0,5,0);
                    break;
                case 12:
                    mDueTime = new TimeSpan(0,10,0);
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

            mSampleFile = textBoxSampleFile.Text;
            mDestinationPath = textBoxDestinationPath.Text;

            sTimer = new Threading.Timer(_TimerHandler);
            _StartHandler();

            textBoxSampleFile.Enabled = false;
            textBoxDestinationPath.Enabled = false;
            comboBoxInterval.Enabled = false;
            buttonBrowseDestinationPath.Enabled = false;
            buttonBrowseSamplePath.Enabled = false;
            buttonStart.Text = Resources.Stop;
            buttonStart.Click += _Stop;
            buttonStart.Click -= _Start;
        }

        private void _Stop(object sender, EventArgs e)
        {
            _StopHandler();

            textBoxSampleFile.Enabled = true;
            textBoxDestinationPath.Enabled = true;
            comboBoxInterval.Enabled = true;
            buttonBrowseDestinationPath.Enabled = true;
            buttonBrowseSamplePath.Enabled = true;
            buttonStart.Text = Resources.Start;
            buttonStart.Click += _Start;
            buttonStart.Click -= _Stop;
        }

        private void _TimerHandler(object state)
        {
            _WriteFile();

            if (!sIsHandlerDisposed)
                _StartHandler();
        }

        private void _WriteFile()
        {
            throw new NotImplementedException();
        }

        private static void _StopHandler()
        {
            sIsHandlerDisposed = true;
            sTimer.Dispose();
        }

        private void _StartHandler()
        {
            sIsHandlerDisposed = false;
            sTimer.Change(mDueTime, Threading.Timeout.InfiniteTimeSpan);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LimsHelper;
using Threading = System.Threading;

namespace LimsVisualizer
{
    public partial class MainForm : Form
    {
        public static Logger LogWriter = new Logger();
        public static bool HeadersWritten;
        private Document mDocument = new Document();
        private readonly Parser mParser = new Parser();
        private static bool sIsHandlerDisposed = true;
        private static ExcelHelper sExcelHelper;
        private static MainForm sInstance;
        private static Threading.Timer sTimer;
        private static ActiveProduct sActiveProduct = new ActiveProduct { Product = new Product { Id = Guid.NewGuid() } };
        private static LimsVisualizerSettings sLimsVisualizerSettings;
        private static readonly SettingsProvider sSettingsProvider = new SettingsProvider();

        public MainForm()
        {
            sInstance = this;
            var application = System.Reflection.Assembly.GetExecutingAssembly().GetName();

            LogWriter.ApplicationName = application.Name;
            LogWriter.ApplicationVersion = application.Version.ToString();
            LogWriter.LogFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), application.Name);
            mParser.Logger = LogWriter;
            sSettingsProvider.ApplicationName = application.Name;
            sSettingsProvider.LogWriter = LogWriter;
            InitializeComponent();
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

        private void _ButtonBrowseClick(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = sLimsVisualizerSettings.FilePath;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                sLimsVisualizerSettings.FilePath = textBoxPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void _ButtonStartClick(object sender, EventArgs e)
        {
            _ChangeStateOfAllControls();

            if (!Directory.Exists(sLimsVisualizerSettings.FilePath))
            {
                var popupMessage = new PopupMessage { Type = PopupMessageType.Info };
                popupMessage.ShowDialog(this,
                    string.Format("The provided path '{0}' does not exist!", sLimsVisualizerSettings.FilePath));
                _ChangeStateOfAllControls();
                return;
            }

            sExcelHelper = new ExcelHelper();
            sTimer = new Threading.Timer(_TimerHandler);

            LogWriter.WriteDebugMessage(string.Format("Starting Timer. FilePath: '{0}' Interval: '{1}'",
                sLimsVisualizerSettings.FilePath,
                sLimsVisualizerSettings.DueTime));
            _StartHandler();
            LogWriter.WriteDebugMessage(string.Format("Started Timer successfully."));
        }

        private void _ButtonStopClick(object sender, EventArgs e)
        {
            LogWriter.WriteDebugMessage(string.Format("Stopping Timer."));
            _StopHandler();
            LogWriter.WriteDebugMessage(string.Format("Stopped Timer successfully."));
            sExcelHelper = null;
            _ChangeStateOfAllControls();
        }

        private void _TimerHandler(object state)
        {
            try
            {
                _CheckForFile();

                //Only start handler if it is not disposed
                if (!sIsHandlerDisposed)
                    _StartHandler();
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Error in _TimerHandler!");
                LogWriter.WriteException(exception);
                ShowErrorMessage(exception);
            }
        }

        private static void _StopHandler()
        {
            sIsHandlerDisposed = true;
            sActiveProduct = null;
            sTimer.Dispose();
            sExcelHelper.Dispose();
            HeadersWritten = false;
        }

        private void _StartHandler()
        {
            sIsHandlerDisposed = false;
            sTimer.Change(sLimsVisualizerSettings.DueTime, Timeout.InfiniteTimeSpan);
        }

        private void _CheckForFile()
        {
            var files = _SortFileListByExtension(Directory.GetFiles(sLimsVisualizerSettings.FilePath));

            foreach (var file in files)
            {
                if (sIsHandlerDisposed)
                    return;
                
                try
                {
                    mDocument = mParser.ParseFile(file);
                }
                catch (Exception exception)
                {
                    ShowErrorMessage(exception);
                }

                if ((sActiveProduct == null) || (sActiveProduct.Product.Id != mDocument.Summary.ActiveProduct.Product.Id))
                {
                    sExcelHelper.StartExcel();
                    sExcelHelper.CreateWorkbook();
                    sActiveProduct = mDocument.Summary.ActiveProduct;
                }

                if (!HeadersWritten)
                {
                    sExcelHelper.AddHeaders(mDocument);
                    HeadersWritten = true;
                }

                sExcelHelper.AddMeasurementValues(mDocument);
                sExcelHelper.AddMiscellaneousValues(mDocument);

                LogWriter.WriteDebugMessage(string.Format("Deleting file: '{0}'", file));
                File.Delete(file);
                LogWriter.WriteDebugMessage("Deleted file successfully.");
            }
        }

        private void _ChangeStateOfAllControls()
        {
            if(InvokeRequired)
            {
                Invoke(new Action(_ChangeStateOfAllControls));
                return;
            }

            ControlBox = !ControlBox;
            buttonStart.Enabled = !buttonStart.Enabled;
            buttonStop.Enabled = !buttonStop.Enabled;
            buttonBrowse.Enabled = !buttonBrowse.Enabled;
            textBoxPath.Enabled = !textBoxPath.Enabled;
            trackBarCheckFrequency.Enabled = !trackBarCheckFrequency.Enabled;
            numericUpDownCheckFrequency.Enabled = !numericUpDownCheckFrequency.Enabled;
        }

        private void _TrackBarCheckFrequencyValueChanged(object sender, EventArgs e)
        {
            numericUpDownCheckFrequency.Value = trackBarCheckFrequency.Value;
            sLimsVisualizerSettings.DueTime = new TimeSpan(0, 0, 0, 0, trackBarCheckFrequency.Value);
        }

        private void _NumericUpDownCheckFrequencyValueChanged(object sender, EventArgs e)
        {
            trackBarCheckFrequency.Value = Convert.ToInt16(numericUpDownCheckFrequency.Value);
            sLimsVisualizerSettings.DueTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(numericUpDownCheckFrequency.Value));
        }

        private void _FormMainLoad(object sender, EventArgs e)
        {
            LogWriter.Initialize();
            sSettingsProvider.GetSettings(out sLimsVisualizerSettings);
            textBoxPath.Text = folderBrowserDialog.SelectedPath = sLimsVisualizerSettings.FilePath;
            trackBarCheckFrequency.Value = (int)sLimsVisualizerSettings.DueTime.TotalMilliseconds;
            numericUpDownCheckFrequency.Value = (decimal)sLimsVisualizerSettings.DueTime.TotalMilliseconds;
            buttonStop.Enabled = false;
            textBoxPath.Text = sLimsVisualizerSettings.FilePath;
        }

        private void _FormMainClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                sLimsVisualizerSettings.FilePath = textBoxPath.Text;
                sLimsVisualizerSettings.DueTime = new TimeSpan(0, 0, 0, 0, (int)numericUpDownCheckFrequency.Value);

                sSettingsProvider.WriteSettings(sLimsVisualizerSettings);
                LogWriter.Dispose();
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        private IEnumerable<string> _SortFileListByExtension(string[] files)
        {
            try
            {
                if (files.Length > 1)
                {
                    var extensionArray = new long[files.Length];
                    var filePathAndNameWithoutExtension = Path.Combine(Path.GetDirectoryName(files[0]), Path.GetFileNameWithoutExtension(files[0]) + ".");

                    for (var i = 0; i < extensionArray.Length; i++)
                    {
                        var extension = Path.GetExtension(files[i]);

                        try
                        {
                            if (!string.IsNullOrEmpty(extension))
                            {
                                extensionArray[i] = Convert.ToInt64(extension.Replace(".", ""));
                            }
                        }
                        catch (FormatException)
                        {
                            LogWriter.WriteDebugMessage(string.Format("Format Exception occured while sorting the file array. Caused by File: {0}", files[i]));
                            return files;
                        }
                    }

                    Array.Sort(extensionArray);

                    for (var i = 0; i < extensionArray.Length; i++)
                    {
                        files[i] = string.Concat(filePathAndNameWithoutExtension, extensionArray[i]);
                    }
                }
                return files;
            }
            catch (Exception exception)
            {
                LogWriter.WriteFailureMessage("Sorting File Array failed!");
                LogWriter.WriteException(exception);
                ShowErrorMessage(exception);
            }

            return null;
        }

        private void _TextBoxPathTextChanged(object sender, EventArgs e)
        {
            sLimsVisualizerSettings.FilePath = textBoxPath.Text;
        }
    }
}

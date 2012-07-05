using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LimsVisualizer.Properties;
using Threading = System.Threading;

namespace LimsVisualizer
{
    public partial class MainForm : Form
    {
        public static Logger LogWriter = new Logger();
        private readonly SettingsProvider mSettingsProvider = new SettingsProvider();
        private readonly Parser mParser = new Parser();
        private Document mDocument = new Document();
        public static bool HeadersWritten;
        private static bool sIsHandlerDisposed;
        private static ExcelHelper sExcelHelper;
        private static MainForm sInstance;
        private static Threading.Timer sTimer;
        private static ActiveProduct sActiveProduct = new ActiveProduct { Product = new Product { Id = Guid.NewGuid() } };

        public static string Path { get; set; }
        public static int DueTime { get; set; }

        public MainForm()
        {
            sInstance = this;
            InitializeComponent();
        }

        public static void ShowErrorMessage(Exception exception)
        {
            var errorMessage = new ErrorMessage { Type = Resources.Error };
            _StopHandler();
            errorMessage.ShowErrorDialog(ActiveForm, exception.Message, exception.Data.ToString());
            sInstance._ChangeStateOfAllControls();
        }

        private void _ButtonBrowseClick(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = Path;

            if (folderBrowserDialog.ShowDialog()== DialogResult.OK)
            {
                Path = textBoxPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void _ButtonStartClick(object sender, EventArgs e)
        {
            _ChangeStateOfAllControls();

            if (!Directory.Exists(Path))
            {
                MessageBox.Show(string.Format("The provided path '{0}' does not exist!", Path), Resources.Info,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ChangeStateOfAllControls();
                return;
            }

            sExcelHelper = new ExcelHelper();
            sTimer = new Threading.Timer(_TimerHandler);

            LogWriter.WriteDebugMessage(string.Format("Starting Timer. Path: '{0}' Interval: '{1}'",Path,DueTime));
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
            sTimer.Change(DueTime, Timeout.Infinite);
        }

        private void _CheckForFile()
        {
            var files = _SortFileListByExtension(Directory.GetFiles(Path));

            foreach (var file in files)
            {
                if (sIsHandlerDisposed)
                    return;

                mDocument = mParser.ParseFile(file);

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
            DueTime = trackBarCheckFrequency.Value;
        }

        private void _NumericUpDownCheckFrequencyValueChanged(object sender, EventArgs e)
        {
            trackBarCheckFrequency.Value = Convert.ToInt16(numericUpDownCheckFrequency.Value);
            DueTime = Convert.ToInt16(numericUpDownCheckFrequency.Value);
        }

        private void _FormMainLoad(object sender, EventArgs e)
        {
            LogWriter.Initialize();
            mSettingsProvider.GetSettings();
            textBoxPath.Text = folderBrowserDialog.SelectedPath = Path;
            trackBarCheckFrequency.Value = DueTime;
            numericUpDownCheckFrequency.Value = DueTime;
            buttonStop.Enabled = false;
            textBoxPath.Text = Path;
        }

        private void _FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            mSettingsProvider.WriteSettings();
            LogWriter.Dispose();
        }

        private IEnumerable<string> _SortFileListByExtension(string[] files)
        {
            try
            {
                if (files.Length > 1)
                {
                    var extensionArray = new long[files.Length];
// ReSharper disable AssignNullToNotNullAttribute
                    var filePathAndNameWithoutExtension = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(files[0]), System.IO.Path.GetFileNameWithoutExtension(files[0]) + ".");
// ReSharper restore AssignNullToNotNullAttribute

                    for (var i = 0; i < extensionArray.Length; i++)
                    {
                        var extension = System.IO.Path.GetExtension(files[i]);

                        if (extension != null)
                        {
                            extensionArray[i] = Convert.ToInt64(extension.Replace(".", ""));
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
            Path = textBoxPath.Text;
        }
    }
}

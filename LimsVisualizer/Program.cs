using System;
using System.Windows.Forms;

namespace LimsVisualizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
// ReSharper disable InconsistentNaming
        static void Main()
// ReSharper restore InconsistentNaming
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}

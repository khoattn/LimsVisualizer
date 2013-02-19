using System;
using System.Drawing;
using System.Windows.Forms;
using LimsHelper.Properties;

namespace LimsHelper
{
    public partial class ErrorMessage : Form
    {
        private string mMessage;
        private string mDetails;

        public ErrorMessage()
        {
            InitializeComponent();
        }

        public void ShowErrorDialog(Form owner, string message, string details)
        {
            mMessage = message;
            mDetails = details;
            icon.Image = Resources.error_128x128;
            labelTitle.Text = Type.ToUpper();
            labelMessage.Text = mMessage;
            textBoxDetails.Text = mDetails;

            ShowDialog(owner);
        }

        public void ShowInformationDialog(Form owner, string message)
        {
            mMessage = message;
            icon.Image = Resources.information_128x128;
            labelTitle.Text = Type.ToUpper();
            labelMessage.Text = mMessage;
            buttonDetails.Visible = false;

            ShowDialog(owner);
        }

        public string Type { get; set; }

        private void _ButtonDetailsClick(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2Collapsed == false)
            {
                var tempSize = splitContainer1.Panel1.Size;
                Size = new Size(679, 163);
                splitContainer1.Panel2Collapsed = true;
                buttonDetails.Text = Resources.ShowDetails;
                splitContainer1.SplitterDistance = tempSize.Height;
            }
            else
            {
                var tempSize = splitContainer1.Panel1.Size;
                Size = new Size(679, 474);
                splitContainer1.Panel2Collapsed = false;
                buttonDetails.Text = Resources.HideDetails;
                splitContainer1.SplitterDistance = tempSize.Height;
            }
        }
    }
}

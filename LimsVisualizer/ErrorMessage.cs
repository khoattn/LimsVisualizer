using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LimsVisualizer
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
            icon.Image = Properties.Resources.error_128x128;
            labelTitle.Text = Type.ToUpper();
            labelMessage.Text = mMessage;
            textBoxDetails.Text = mDetails;

            ShowDialog(owner);
        }

        public void ShowInformationDialog(Form owner, string message)
        {
            mMessage = message;
            icon.Image = Properties.Resources.information_128x128;
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
                buttonDetails.Text = "Show Details";
                splitContainer1.SplitterDistance = tempSize.Height;
            }
            else
            {
                var tempSize = splitContainer1.Panel1.Size;
                Size = new Size(679, 474);
                splitContainer1.Panel2Collapsed = false;
                buttonDetails.Text = "Hide Details";
                splitContainer1.SplitterDistance = tempSize.Height;
            }
        }
    }
}

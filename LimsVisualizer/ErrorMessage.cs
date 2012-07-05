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
        
        public void ShowDialog(Form owner, string message, string details)
        {
            mMessage = message;
            mDetails = details;

            if (Type == "error")
            {
                icon.Image = Properties.Resources.error_128x128;
            }
            else
            {
                icon.Image = Properties.Resources.information_128x128;
            }

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
                button2.Text = "Show Details";
                splitContainer1.SplitterDistance = tempSize.Height;
            }
            else
            {
                var tempSize = splitContainer1.Panel1.Size;
                Size = new Size(679, 474);
                splitContainer1.Panel2Collapsed = false;
                button2.Text = "Hide Details";
                splitContainer1.SplitterDistance = tempSize.Height;
            }
        }
    }
}

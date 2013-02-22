using System;
using System.Drawing;
using System.Windows.Forms;
using LimsHelper.Properties;

namespace LimsHelper
{
    public partial class PopupMessage : Form
    {
        public PopupMessage()
        {
            InitializeComponent();
        }

        public void ShowDialog(Form owner, string message, string details)
        {
            icon.Image = (Type == PopupMessageType.Error) ? Resources.ErrorIcon : Resources.InformationIcon;
            labelTitle.Text = (Type == PopupMessageType.Error) ? Resources.ErrorTitle.ToUpper() : Resources.InfoTitle.ToUpper();
            labelMessage.Text = message;
            textBoxDetails.Text = details;

            ShowDialog(owner);
        }

        public void ShowDialog(Form owner, string message)
        {
            icon.Image =(Type == PopupMessageType.Error) ? Resources.ErrorIcon : Resources.InformationIcon;
            labelTitle.Text = (Type == PopupMessageType.Error) ? Resources.ErrorTitle.ToUpper() : Resources.InfoTitle.ToUpper();
            labelMessage.Text = message;
            buttonDetails.Visible = false;

            ShowDialog(owner);
        }

        public PopupMessageType Type { private get; set; }

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

    public enum PopupMessageType
    {
        Error,
        Info
    }
}

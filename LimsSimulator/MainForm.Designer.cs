namespace LimsSimulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSampleFile = new System.Windows.Forms.TextBox();
            this.textBoxDestinationPath = new System.Windows.Forms.TextBox();
            this.buttonBrowseSamplePath = new System.Windows.Forms.Button();
            this.buttonBrowseDestinationPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.openFileDialogSampleFile = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogDestinationPath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sample File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination Path:";
            // 
            // textBoxSampleFile
            // 
            this.textBoxSampleFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSampleFile.Location = new System.Drawing.Point(15, 25);
            this.textBoxSampleFile.Name = "textBoxSampleFile";
            this.textBoxSampleFile.Size = new System.Drawing.Size(319, 20);
            this.textBoxSampleFile.TabIndex = 2;
            // 
            // textBoxDestinationPath
            // 
            this.textBoxDestinationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDestinationPath.Location = new System.Drawing.Point(15, 64);
            this.textBoxDestinationPath.Name = "textBoxDestinationPath";
            this.textBoxDestinationPath.Size = new System.Drawing.Size(319, 20);
            this.textBoxDestinationPath.TabIndex = 3;
            // 
            // buttonBrowseSamplePath
            // 
            this.buttonBrowseSamplePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseSamplePath.Location = new System.Drawing.Point(340, 23);
            this.buttonBrowseSamplePath.Name = "buttonBrowseSamplePath";
            this.buttonBrowseSamplePath.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseSamplePath.TabIndex = 4;
            this.buttonBrowseSamplePath.Text = "Browse";
            this.buttonBrowseSamplePath.UseVisualStyleBackColor = true;
            this.buttonBrowseSamplePath.Click += new System.EventHandler(this._ButtonBrowseSamplePathClick);
            // 
            // buttonBrowseDestinationPath
            // 
            this.buttonBrowseDestinationPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseDestinationPath.Location = new System.Drawing.Point(340, 62);
            this.buttonBrowseDestinationPath.Name = "buttonBrowseDestinationPath";
            this.buttonBrowseDestinationPath.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseDestinationPath.TabIndex = 5;
            this.buttonBrowseDestinationPath.Text = "Browse";
            this.buttonBrowseDestinationPath.UseVisualStyleBackColor = true;
            this.buttonBrowseDestinationPath.Click += new System.EventHandler(this._ButtonBrowseDestinationPathClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Interval:";
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Location = new System.Drawing.Point(15, 103);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(121, 21);
            this.comboBoxInterval.TabIndex = 8;
            this.comboBoxInterval.SelectedIndexChanged += new System.EventHandler(this._ComboBoxIntervalSelectedIndexChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(340, 101);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 9;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this._Start);
            // 
            // openFileDialogSampleFile
            // 
            this.openFileDialogSampleFile.Filter = "XML files|*.xml|All files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 136);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.buttonBrowseDestinationPath);
            this.Controls.Add(this.buttonBrowseSamplePath);
            this.Controls.Add(this.textBoxDestinationPath);
            this.Controls.Add(this.textBoxSampleFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Lims Simulator";
            this.Load += new System.EventHandler(this._LimsSimulatorLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSampleFile;
        private System.Windows.Forms.TextBox textBoxDestinationPath;
        private System.Windows.Forms.Button buttonBrowseSamplePath;
        private System.Windows.Forms.Button buttonBrowseDestinationPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxInterval;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.OpenFileDialog openFileDialogSampleFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDestinationPath;
    }
}


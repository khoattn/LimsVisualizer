using System;
using System.Collections.Generic;
using System.IO;

namespace LimsVisualizer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.trackBarCheckFrequency = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.numericUpDownCheckFrequency = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCheckFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheckFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(422, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "&Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this._ButtonStartClick);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(422, 41);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "S&top";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this._ButtonStopClick);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.SelectedPath = "C:\\Lims";
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // trackBarCheckFrequency
            // 
            this.trackBarCheckFrequency.Location = new System.Drawing.Point(12, 28);
            this.trackBarCheckFrequency.Maximum = 10000;
            this.trackBarCheckFrequency.Minimum = 500;
            this.trackBarCheckFrequency.Name = "trackBarCheckFrequency";
            this.trackBarCheckFrequency.Size = new System.Drawing.Size(315, 45);
            this.trackBarCheckFrequency.TabIndex = 2;
            this.trackBarCheckFrequency.TickFrequency = 200;
            this.trackBarCheckFrequency.Value = 500;
            this.trackBarCheckFrequency.ValueChanged += new System.EventHandler(this._TrackBarCheckFrequencyValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Check Frequency";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(12, 79);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(404, 20);
            this.textBoxPath.TabIndex = 5;
            this.textBoxPath.TextChanged += new System.EventHandler(this._TextBoxPathTextChanged);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(422, 77);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 6;
            this.buttonBrowse.Text = "&Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this._ButtonBrowseClick);
            // 
            // numericUpDownCheckFrequency
            // 
            this.numericUpDownCheckFrequency.Location = new System.Drawing.Point(333, 28);
            this.numericUpDownCheckFrequency.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCheckFrequency.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownCheckFrequency.Name = "numericUpDownCheckFrequency";
            this.numericUpDownCheckFrequency.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownCheckFrequency.TabIndex = 7;
            this.numericUpDownCheckFrequency.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownCheckFrequency.ValueChanged += new System.EventHandler(this._NumericUpDownCheckFrequencyValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(394, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "mS";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 111);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownCheckFrequency);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarCheckFrequency);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Lims Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._FormMainFormClosing);
            this.Load += new System.EventHandler(this._FormMainLoad);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCheckFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheckFrequency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TrackBar trackBarCheckFrequency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.NumericUpDown numericUpDownCheckFrequency;
        private System.Windows.Forms.Label label2;
    }
}


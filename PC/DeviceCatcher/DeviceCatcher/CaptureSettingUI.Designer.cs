﻿namespace DeviceCatcher
{
    partial class CaptureSettingUI
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
            this.settingsPanel = new System.Windows.Forms.GroupBox();
            this.rotateBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.rotateBtn);
            this.settingsPanel.Controls.Add(this.okBtn);
            this.settingsPanel.Location = new System.Drawing.Point(68, 79);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(180, 134);
            this.settingsPanel.TabIndex = 2;
            this.settingsPanel.TabStop = false;
            this.settingsPanel.Text = "Settings";
            // 
            // rotateBtn
            // 
            this.rotateBtn.Location = new System.Drawing.Point(46, 76);
            this.rotateBtn.Name = "rotateBtn";
            this.rotateBtn.Size = new System.Drawing.Size(88, 23);
            this.rotateBtn.TabIndex = 3;
            this.rotateBtn.Text = "Rotate";
            this.rotateBtn.UseVisualStyleBackColor = true;
            this.rotateBtn.Click += new System.EventHandler(this.rotateBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.okBtn.Location = new System.Drawing.Point(46, 37);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(88, 23);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "Hide";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // CaptureSettingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(318, 287);
            this.Controls.Add(this.settingsPanel);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaptureSettingUI";
            this.Opacity = 0.5D;
            this.Text = "CaptureSettingUI";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CaptureSettingUI_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CaptureSettingUI_MouseMove);
            this.Resize += new System.EventHandler(this.CaptureSettingUI_Resize);
            this.settingsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox settingsPanel;
        private System.Windows.Forms.Button rotateBtn;
        private System.Windows.Forms.Button okBtn;

    }
}
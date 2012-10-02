using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceCatcher
{
    public partial class CaptureSettingUI : Form
    {
        private int mDeviceWidth = 320;
        private int mDeviceHeight = 480;
        public CaptureSettingUI()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CaptureSettingUI_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = e.Location.ToString();
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - downX, this.Location.Y + e.Y - downY);
            }
        }

        private void CaptureSettingUI_MouseDown(object sender, MouseEventArgs e)
        {
            downX = e.X;
            downY = e.Y;
        }

        private int downX = 0;
        private int downY = 0;

        private void CaptureSettingUI_Resize(object sender, EventArgs e)
        {
            this.settingsPanel.Location = new Point((this.Width - settingsPanel.Width) / 2, (this.Height - settingsPanel.Height) / 2);
        }

        private void rotateBtn_Click(object sender, EventArgs e)
        {
            int tmp = this.Width;
            this.Width = this.Height;
            this.Height = tmp;
        }

        public bool Rotated
        {
            get
            {
                return this.Width != this.mDeviceWidth;
            }
        }

        public void setSize(int w, int h)
        {
            this.mDeviceWidth = w;
            this.mDeviceHeight = h;
            this.Size = new Size(w, h);
        }
    }
}

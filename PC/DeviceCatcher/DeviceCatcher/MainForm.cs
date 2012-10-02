using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DeviceCatcher
{
    public partial class MainForm : Form
    {
        private Thread mScanThread = null;
        private Thread mSendThread = null;
        private List<EndPoint> mDeviceList = new List<EndPoint>();
        private IPEndPoint mDeviceEndPoint = null;
        private Socket mSendSocket = null;
        private readonly string STOPSCANNING = "Stop Scanning...";
        private readonly string SCANDEVICE = "Scan Device";
        private bool mRotated = false;
        private int curDeviceWidth = 320;
        private int curDeviceHeight = 480;
        public MainForm()
        {
            InitializeComponent();
            mScanThread = new Thread(new ThreadStart(scan));
            mScanThread.Start();
        }

        private void findBtn_Click(object sender, EventArgs e)
        {
            //mScanThread = new Thread(new ThreadStart(scan));
            //mScanThread.Start();
            //scan();
        }

        private void scan()
        {
            //初始化一个Scoket协议
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //初始化一个侦听局域网内部所有IP和指定端口
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9095);
            EndPoint ep = (EndPoint)iep;
            //绑定这个实例
            socket.Bind(iep);
            //socket.SendTimeout = 200;
            //int cnt = 0;
            while (true)
            {
                Thread.Sleep(200);
                //this.sendToBtn.Text = cnt.ToString();
                //cnt++;
                //设置缓冲数据流
                byte[] buffer = new byte[1024];
                try
                {
                    //接收数据,并确把数据设置到缓冲流里面
                    if (socket.ReceiveFrom(buffer, ref ep) > 0)
                    {
                        //String info = new string(buffer);
                        if (!mDeviceList.Contains(ep))
                        {
                            mDeviceList.Add(ep);
                            string info = Encoding.ASCII.GetString(buffer);
                            string[] sAll = info.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            if (sAll.Length > 2)
                            {
                                deviceList.Items.Add(new ListViewItem(new string[] { ep.ToString(), sAll[0] + "x" + sAll[1], sAll[2] }));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.sendToBtn.Text = ex.Message;
                }
            }

            this.sendToBtn.Text = "TTT";
        }

        private void startScanning()
        {
            if (null != mScanThread)
            {
                //There might be bug here.
                return;
            }
            deviceList.Items.Clear();
            mScanThread = new Thread(new ThreadStart(scan));
            mScanThread.Start();
            scanBtn.Text = STOPSCANNING;
        }

        private bool stopScanning()
        {
            if (null != mScanThread)
            {
                if (mScanThread.IsAlive)
                {
                    mScanThread.Abort();
                }
                mScanThread = null;
                scanBtn.Text = SCANDEVICE;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void shutdownConnections(Socket socket)
        {
            if (null != mSendSocket)
            {
                try
                {
                    mSendSocket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception)
                {
                }
                try
                {
                    mSendSocket.Close();
                }
                catch (Exception)
                {
                }
                mSendSocket = null;
            }
        }

        private void sendToBtn_Click(object sender, EventArgs e)
        {
            stopScanning();
            mSendThread = new Thread(new ThreadStart(test));
            mSendThread.Start();
        }

        private void waitForReceiveComplete()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9097);
            EndPoint ep = (EndPoint)iep;
            socket.Bind(iep);
            byte[] buffer = new byte[1024];
            socket.ReceiveFrom(buffer, ref ep);
            shutdownConnections(socket);
        }

        private void test()
        {
            //int lastCode = 0;
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    //Send to device
                    this.Text = "1";
                    if (null == mDeviceEndPoint)
                    {
                        this.Text = "2";
                        String address = deviceList.SelectedItems[0].Text;
                        String ipStr = address.Split(new char[] { ':' })[0];
                        this.Text = "3";
                        mDeviceEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), 9096);
                    }
                    this.Text = "4";
                    if (null == mSendSocket)
                    {
                        this.Text = "5";
                        mSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        this.Text = "6";
                        mSendSocket.Connect(mDeviceEndPoint);
                        this.Text = "7";
                        mSendSocket.SendTimeout = 20;

                        //Socket udpTest = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
                        //udpTest.co
                        this.Text = DateTime.Now.ToLongTimeString();
                    }

                    byte[] buffer = getScreenImageData();
                    mSendSocket.Send(buffer);
                    shutdownConnections(mSendSocket);

                    //Wait for complete reply
                    //waitForReceiveComplete();
                }
                catch (Exception)
                {
                    shutdownConnections(mSendSocket);
                }
                aboutBtn.Text = DateTime.Now.ToLongTimeString();
            }
        }

        private byte[] getScreenImageData()
        {
            //Screen scr = Screen.PrimaryScreen;
            //Rectangle rc = scr.Bounds;
            //int iWidth = rc.Width;
            //int iHeight = rc.Height;
            //Bitmap myImage = new Bitmap(iWidth, iHeight);
            //Graphics g1 = Graphics.FromImage(myImage);
            //if (null != captureUI)
            //{
            //    g1.CopyFromScreen(captureUI.Location, new Point(0, 0), captureUI.Size);
            //}
            //else
            //{
            //    g1.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(480, 800));
            //}
            int w = 100;
            int h = 100;
            if (null != captureUI)
            {
                w = captureUI.Width;
                h = captureUI.Height;
            }

            Bitmap myImage = new Bitmap(w, h);
            Graphics g1 = Graphics.FromImage(myImage);
            if (null != captureUI)
            {
                g1.CopyFromScreen(captureUI.Location, new Point(0, 0), captureUI.Size);
                if (captureUI.Rotated)
                {
                    //Try rotate
                    myImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    //this.BackgroundImage = myImage;
                }
            }
            else
            {
                g1.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(w, h));
            }
            MemoryStream ms = new MemoryStream();

            myImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Close();
            return ms.ToArray();
        }

        private byte[] testRotate()
        {
            int w = 100;
            int h = 100;
            if (null != captureUI)
            {
                w = captureUI.Width;
                h = captureUI.Height;
            }

            Bitmap myImage = new Bitmap(w, h);
            Graphics g1 = Graphics.FromImage(myImage);
            bool rotated = false;
            if (null != captureUI)
            {
                g1.CopyFromScreen(captureUI.Location, new Point(0, 0), captureUI.Size);
                rotated = captureUI.Rotated;
            }
            else
            {
                g1.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(w, h));
            }
            MemoryStream ms = new MemoryStream();
            if (rotated)
            {
                //Try rotate
                //myImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            myImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            ms.Close();
            return ms.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("World", "Hello", MessageBoxButtons.OK, MessageBoxIcon.Information);
            testRotate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null == captureUI)
            {
                captureUI = new CaptureSettingUI();
            }
            if (deviceList.SelectedItems.Count > 0)
            {
                String address = deviceList.SelectedItems[0].SubItems[1].Text;
                String[] sAll = address.Split(new char[] { 'x' });
                if (sAll.Length > 1)
                {
                    curDeviceWidth = int.Parse(sAll[0]);
                    curDeviceHeight = int.Parse(sAll[1]);
                }
            }
            captureUI.setSize(curDeviceWidth, curDeviceHeight);
            captureUI.Show();
        }

        CaptureSettingUI captureUI = null;

        private void scanBtn_Click(object sender, EventArgs e)
        {
            if (!stopScanning())
            {
                startScanning();
            }
        }
    }
}

using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Ping_Log
{
    public partial class Form1 : Form
    {
        public Form1()
        {           
            InitializeComponent();           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start"))
            {
                WriteLog("=====================Start=====================");
                timer1.Enabled = true;
                button1.Text = "End";
            }
            else
            {
                WriteLog("=====================End=====================");
                button1.Text = "Start";
                timer1.Enabled = false;
            }
        }

        private void Start()
        {
            //构造Ping实例
            Ping ping = new Ping();

            //Ping选项设置，用于控制如何传输数据包
            PingOptions poptions = new PingOptions();
            poptions.DontFragment = true;

            //测试数据
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            Byte[] buffer = Encoding.ASCII.GetBytes(data);

            //设置超时时间
            int timeout = 1000;

            //调用同步send方法发送数据，将返回结果保存至PingReply实例
            //此处如果直接ping IP的话，先引用命名空间using System.Net;
            //然后代码改为：PingReply pingreply = ping.Send(IPadress.Parse("192.168.1.1"),timeout,buffer,poptions);

            PingReply pingreply = ping.Send(IPAddress.Parse(IP.Text), timeout, buffer, poptions);
            string message = "";
            if (pingreply.Status == IPStatus.Success)
            {
                message = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.f") + "\t回覆自 " + pingreply.Address.ToString() + ":\tBuffer size=" + pingreply.Buffer.Length + "\ttime = " + pingreply.RoundtripTime + "\tTTL=" + pingreply.Options.Ttl + "\tSuccess";
                //msg.Items.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.f") + "\r 回覆自 "+ pingreply .Address.ToString()+ ":\n Buffer size=" + pingreply.Buffer.Length + "\n time = "+ pingreply.RoundtripTime + "\n TTL="+ pingreply .Options.Ttl+ "\nSuccess");
                //MessageBox.Show("网络通畅", "提示");
            }
            else
            {
                message = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.f") + " Fail";
               // msg.Items.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.f")+" Fail");
               // MessageBox.Show("网络不通", "提示");
            }
            msg.Items.Add(message);
            msg.SelectedIndex = msg.Items.Count - 1;
            WriteLog(message);

        }
        private void WriteLog(string msg)
        {
            StreamWriter sw;
            string path = IP.Text.Trim() + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (File.Exists(path))
            {
                sw = File.AppendText(path);
            }
            else
            {
                sw = new StreamWriter(path);
            }
            sw.WriteLine(msg);
            sw.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (msg.Items.Count >= 60)
            {
                msg.Items.Clear();                
            }
            CallGC();
            Start();
        }
        private void CallGC()
        {
            GC.Collect();
        }
    }
}

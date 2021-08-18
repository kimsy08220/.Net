using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace _20163118_2
{
    public partial class Form1 : Form
    {
        TcpClient tclient;
        Thread th;
        string nick;
      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)                  // 전송 버튼 Click
        {
            SendMSG();
            //textBox1.Text = "";
            textBox1.Clear();
        }

        private void SendMSG()
        {
            try
            {
                if (!textBox1.Text.Equals("") && tclient.Connected)
                {
                    NetworkStream ns = tclient.GetStream();

                    Encoding ASCII = Encoding.ASCII;
                    Byte[] ByteGet = ASCII.GetBytes(textBox1.Text);
                    Byte[] RecvBytes = new Byte[1024];

                    if (tclient != null)
                        ns.Write(ByteGet, 0, ByteGet.Length);           // Write() : Server로 메시지 내용을 전송
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Exception Thrown : " + ex.ToString());
            }
            finally { textBox1.Clear(); }
            
        }

        private void button2_Click(object sender, EventArgs e)                  // 서버 연결 버튼 Click
        {
            while (true)
            {
                tclient = new TcpClient(textBox3.Text, int.Parse(textBox4.Text));
                
                // nick까지 받아와서 보냄
                nick = textBox2.Text;

                if (tclient.Connected)
                {
                    th = new Thread(new ThreadStart(RecvMSG));
                    th.IsBackground = true;
                    th.Start();
                    break;
                }
            }
        }

        private void RecvMSG()
        {
            try
            {
                NetworkStream ns = tclient.GetStream();

                Encoding ASCII = Encoding.ASCII;
                Byte[] ByteGet = ASCII.GetBytes(textBox1.Text);
                Byte[] RecvBytes = new Byte[1024];

                while (true)
                {
                    int bytes = ns.Read(RecvBytes, 0, RecvBytes.Length);    // Read() : Server로부터 메세지 내용을 얻어옴
                    
                    if (tclient != null)
                    {
                        if (bytes == 0)
                            this.Close();
                        else
                            listBox1.Items.Add(ASCII.GetString(RecvBytes, 0, bytes));
                    }
                }
            }
            catch (Exception) { }
            finally { textBox1.Clear(); }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tclient.Close();
        }
    }
}

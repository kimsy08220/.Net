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
using System.Runtime.Serialization.Formatters.Binary;

namespace _20163118_2
{
    public partial class Form1 : Form
    {
        TcpClient tclient;
        Thread th;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            while (true)
            {
                tclient = new TcpClient("127.0.0.1", 9000);

                if (tclient.Connected)
                {
                    th = new Thread(new ThreadStart(RecvMSG));
                    th.IsBackground = true;
                    th.Start();
                    break;
                }
            }
        }

        private void RecvMSG()      // 얻어와서 listbox에 쓰는 역할
        {
            try
            {
                NetworkStream ns = tclient.GetStream();

                Encoding ASCII = Encoding.ASCII;
                Byte[] ByteGet = ASCII.GetBytes(textBox1.Text);
                Byte[] RecvBytes = new Byte[1024];

                while (true)
                {
                    int bytes = ns.Read(RecvBytes, 0, RecvBytes.Length);                // Read() : Server로부터 메세지 내용을 얻어옴
                    if (tclient != null)
                    {
                        if (bytes == 0)
                        {
                            this.Close();
                        }
                        else
                        {
                            listBox1.Items.Add(ASCII.GetString(RecvBytes, 0, bytes));
                        }
                    }
                }
            }
            catch (Exception ee) { }
        }

        private void button1_Click(object sender, EventArgs e)              // 전송 버튼
        {
            SendMSG();
            textBox1.Text = "";
        }

        private void SendMSG(){     // 전송하는 역할
            try
            {
                if (!textBox1.Text.Equals("") && tclient.Connected)
                {
                    NetworkStream ns = tclient.GetStream();

                    Encoding ASCII = Encoding.ASCII;
                    Byte[] ByteGet = ASCII.GetBytes(textBox1.Text);
                    Byte[] RecvBytes = new Byte[1024];

                    if (tclient != null)
                    {
                        ns.Write(ByteGet, 0, ByteGet.Length);               // Write() : Server로 메시지 내용을 전송
                    }
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Exception Thrown : " + ex.ToString());
            }
            textBox1.Text = "";
        }

        
        

        
    }
}

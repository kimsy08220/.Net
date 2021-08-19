using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        Byte[] sendByte = new Byte[1024];
        NetworkStream ns;
        string send_msg, receive_msg, nick;  

        public Form1()
        {
            InitializeComponent();
        }

        public void Recieve(Object sender)
        {
            while (true)
            {
                Byte[] receiveByte = new Byte[1024];
                ns.Read(receiveByte, 0, receiveByte.Length);
                receive_msg = Encoding.Default.GetString(receiveByte).Trim('\0');
                receiveByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(receive_msg); //받은 바이트를 문자열로 하여 null 값을 지우고 다시 바이트 형식 변환
                textBox4.AppendText(receive_msg +"" +"\r\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)  //접속
        {
            client = new TcpClient("127.0.0.1", 9000);
            nick = "["+textBox2.Text+"] : ";
            if (client.Connected) //if server connected return true
            {
                ns = client.GetStream();
                send_msg = textBox2.Text;
                sendByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(send_msg);
                ns.Write(sendByte, 0, sendByte.Length);

                Thread thread = new Thread(new ParameterizedThreadStart(Recieve));
                thread.Start(client);
            }
        }

        private void button2_Click(object sender, EventArgs e) //전송
        {
            send_msg = nick + textBox1.Text;
            sendByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(send_msg);
            ns.Write(sendByte, 0, sendByte.Length);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (client != null)
                {
                    Application.ExitThread();
                    Environment.Exit(0);


                    ns.Close();
                    client.Close();
                    Dispose();
                }
                else
                {
                    Application.ExitThread();
                    Environment.Exit(0);
                    ns.Close();
                    client.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

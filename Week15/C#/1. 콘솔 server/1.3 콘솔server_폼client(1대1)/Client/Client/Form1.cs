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

namespace Client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        Byte[] sendByte;
        NetworkStream ns;
        string send_msg,receive_msg;       

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             client = new TcpClient("127.0.0.1",9000);

            if (client.Connected) //if server connected return true
            {
                sendByte = new Byte[1024];
                ns = client.GetStream();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send_msg = textBox1.Text;
            sendByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(send_msg);
            ns.Write(sendByte, 0, sendByte.Length);
            label1.Text =sendByte.Length + "바이트를 전송 하였습니다.";

            Byte[] receiveByte = new Byte[1024];
            ns.Read(receiveByte, 0, receiveByte.Length);
            receive_msg = Encoding.Default.GetString(receiveByte).Trim('\0');
            receiveByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(receive_msg); //받은 바이트를 문자열로 하여 null 값을 지우고 다시 바이트 형식 변환
            label2.Text = "[받은메시지] : " + receive_msg + "\n" + receiveByte.Length + "바이트를 전송 받았습니다.\n";

        }
    }   
}

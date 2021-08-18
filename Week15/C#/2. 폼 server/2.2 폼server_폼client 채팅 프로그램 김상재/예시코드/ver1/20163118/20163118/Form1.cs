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

namespace _20163118
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(string text);
        TcpListener listener;
        TcpClient client;
        Thread t1;
        TcpClient[] clients = new TcpClient[10];
        int size = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)                  // 서버 시작 버튼
        {
            if (button1.Text.Equals("서버 시작"))
            {
                t1 = new Thread(new ParameterizedThreadStart(waitForClient));
                t1.IsBackground = true;
                t1.Start(9000);
                button1.Text = "서버 종료";
            }
            else if (button1.Text.Equals("서버 종료"))
            {
                button1.Text = "서버 시작";
                for (int i = 0; i < size; i++)
                {
                    clients[i].Close();
                }
                listener.Stop();
                t1.Abort();
            }
        }

        private void waitForClient(Object o)
        {
            int port = (int)o;
            listener = new TcpListener(IPAddress.Any, port);
            client = new TcpClient();
            listener.Start();
            
            Thread t;

            try
            {
                while (true)
                {
                    client = listener.AcceptTcpClient();
                    if (client.Connected)
                    {
                        clients[size++] = client;
                        t = new Thread(new ParameterizedThreadStart(threadedWorks));
                        t.IsBackground = true;
                        t.Start(client);
                    }
                    else
                    {
                        listBox1.Items.Add("이전에 소켓이 연결되지 않았습니다. ");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                client.Close();
            }
        }

        private void threadedWorks(Object o)
        {
            TcpClient client = (TcpClient)o;
            Socket s = client.Client;

            string address = s.RemoteEndPoint.ToString();
            string[] array = address.Split(new char[] { ':' });
            byte[] recevbyte = new byte[513];

            NetworkStream ns = client.GetStream();
            Encoding ASCII = Encoding.ASCII;

            listBox1.Items.Add("[TCP 서버] 클라이언트 접속 : IP 주소 = " + array[0] + " 포트번호 = " + array[1]);

            while (true)
            {
                try
                {
                    int bytes = ns.Read(recevbyte, 0, recevbyte.Length);            // Read() : Client로부터 메세지 내용을 얻어옴
                    String strbyte = ASCII.GetString(recevbyte, 0, bytes);

                    if (bytes == 0)     // clients의 자리를 비우는 역할
                    {
                        for (int i = 0; i < size; i++)
                        {
                            if (client == clients[i])
                            {
                                for (; i<size - 1; i++)
                                {
                                    clients[i] = clients[i + 1];
                                }
                                size--;
                                break;
                            }
                        }
                        listBox1.Items.Add("[Client] IP : " + array[0] + " Port : " + array[1] + "종료");
                        break;
                    }
                    else
                    {
                        SendMSG(recevbyte, recevbyte.Length);
                    }
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add("[Client] IP : " + array[0] + " Port : " + array[1] + "종료");
                    break;
                }
            }
            client.Close();
        }

        
        private void SendMSG(byte[] message, int len)
        {
            for (int i = 0; i < size; i++)
            {
                NetworkStream ns = clients[i].GetStream();
                ns.Write(message, 0, message.Length);               // Write() : client[i]로 메시지 내용을 전송
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null)
                listener.Stop();
            if (client != null)
                client.Close();
            if (t1 != null)
                t1.Abort();
        }
    }
}

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
        int cnt = 1;
        byte[] id = new byte[10];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t1 = new Thread(new ParameterizedThreadStart(waitForClient));
            t1.IsBackground = true;
        }

        private void waitForClient(Object o)
        {
            int port = int.Parse(textBox4.Text);
            listener = new TcpListener(IPAddress.Parse(textBox3.Text), port);
            listener.Start();

            client = new TcpClient();
            Thread t;

            try
            {
                while (true)
                {
                    client = listener.AcceptTcpClient();                // 클라이언트 접속을 대기하고 있다가 접속 요청이 오면 이를 받아들여 TcpClient 객체를 생성하여 리턴
                    
                    if (client.Connected)
                    {


                        for (int i = 0; i < 10; i++)                    
                        {
                            if (id[i] == 0)
                            {
                                id[i] = (byte)(char.Parse(cnt.ToString()));
                                cnt++;
                                break;
                            }
                        }
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
                    int bytes = ns.Read(recevbyte, 0, recevbyte.Length);                // Read() : Server로부터 메세지 내용을 얻어옴
                    String strbyte = ASCII.GetString(recevbyte, 0, bytes);
                    if (bytes == 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            if (client == clients[i])
                            {
                                for (; i < size - 1; i++)
                                {
                                    clients[i] = clients[i + 1];
                                }
                                id[i] = 0;
                                size--;
                                break;
                            }
                        }
                        listBox1.Items.Add("[Client] IP : " + array[0] + " Port : " + array[1]+"종료");
                        break;
                    }
                    else
                    {
                        byte[] result = new byte[515];
                        for (int i = 0; i < size; i++)
                        {
                            if (client == clients[i])
                            {
                                result[0] = id[i];
                                result[1] = (byte)(char.Parse(":"));
                                for (int j = 2; j < recevbyte.Length; j++)
                                {
                                    result[j] = recevbyte[j - 2];
                                }
                            }
                        }
                        SendMSG(recevbyte, result.Length);
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
            try
            {
                for (int i = 0; i < size; i++)
                {
                    NetworkStream ns = client.GetStream();
                    ns.Write(message, 0, message.Length);               // Write() : Server로 메시지 내용을 전송
                }           
            }
            catch (Exception) { }
            
        }

        private void button1_Click(object sender, EventArgs e)          // 서버 시작 버튼 Click
        {
            if (button1.Text.Equals("서버 시작"))
            {   
                t1.Start(9000);
                button1.Text = "서버 종료";
            }
            else if (button1.Text.Equals("서버 종료"))
            {
                button1.Text = "서버 시작";
                for (int i = 0; i<size; i++)
                {
                    client.Close();
                }
                listener.Stop();
                t1.Abort();
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;               // TcpListener를 사용하기 위해 선언
using System.Threading;                 // Thread를 사용하기 위해 선언

namespace _190530_TcpServerExWinForm
{
    public partial class Form1 : Form
    {
        //private delegate void SafeCallDelegate(string text);
        TcpListener listener;
        TcpClient client;
        Thread t1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)          // 열기 버튼을 누르면
        {
            if (button1.Text.Equals("열기"))
            {
                int port = int.Parse(textBox2.Text);

                t1 = new Thread(new ParameterizedThreadStart(waitForClient));       // 하나의 인자를 object 형식으로 전달하기 때문에 여러 개의 인자를 전달하기 위해서는 클래스나 구조체를 만들어 객체를 생성해야 함, 스레드로 빼지 않으면 accept 했을 때 block이 되서(리스트 박스 출력) 작동을 안함
                t1.Start(port);
                button1.Text = "닫기";
            }
            else if (button1.Text.Equals("닫기"))
            {
                button1.Text = "열기";
                listener.Stop();
                t1.Abort();                                                         // 스레드 중단
            }
        }

        private void waitForClient(Object o)                                        // 9000 포트를 받음
        {
            int port = (int)o;
            listener = new TcpListener(IPAddress.Any, port);                        // TCP 네트워크 클라이언트에서 연결을 수신
            listener.Start();

            Thread t;
            listBox1.Items.Add("[TCP 서버] 서버가 시작되었습니다.");

            try
            {
                while (true)
                {
                    client = listener.AcceptTcpClient();                            // 클라이언트 접속을 대기하고 있다가 접속 요청이 오면 이를 받아들여 TcpClient 객체를 생성하여 리턴

                    if (client.Connected)
                    {
                        t = new Thread(new ParameterizedThreadStart(threadedWorks));// Echo Thread, 하나의 인자를 object 형식으로 전달하기 때문에 여러 개의 인자를 전달하기 위해서는 클래스나 구조체를 만들어 객체를 생성해야 함
                        t.IsBackground = true;                                      // // true : Background 스레드(메인 스레드가 종료되면 바로 프로세스 종료), false : Foreground 스레드(메인 스레드가 종료되어도 Foreground 스레드가 살아있다면 프로세스가 종료되지 않고 계속 실행)
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
                listBox1.Items.Add("Except Thrown: " + ex.ToString());
                listBox1.Items.Add("[TCP 서버] 서버가 닫혔습니다.");
            }
            finally
            {
                client.Close();
            }
        }

        private void threadedWorks(Object o)
        {
            TcpClient client = (TcpClient)o;                        // TCP 네트워크 서비스에 대한 클라이언트 연결을 제공
            Socket s = client.Client;                               // Client 소켓 생성

            string address = s.RemoteEndPoint.ToString();           // socket의 원격 EndPoint 정보를 조사 ex) 127.0.0.1:51877
            string[] array = address.Split(new char[] { ':' });     // address를 ':'으로 자름, array[0] : 127.0.0.1(IP 주소), array[1] : 51877(포트 번호)
            byte[] recevbyte = new byte[513];                       // 513이 아니어도 됨

            NetworkStream ns = client.GetStream();
            Encoding ASCII = Encoding.ASCII;
            listBox1.Items.Add("[TCP 서버] 클라이언트 접속 : IP 주소 = " + array[0] + " 포트번호 = " + array[1]);

            while (true)
            {
                try
                {
                    int bytes = ns.Read(recevbyte, 0, recevbyte.Length);        // Read() : client로부터 메세지 내용을 얻어옴
                    String strbyte = ASCII.GetString(recevbyte, 0, bytes);

                    if (bytes == 0)
                    {
                        ns.Write(recevbyte, 0, bytes);                          // Read() : client로부터 메세지 내용을 얻어옴
                        listBox1.Items.Add("[TCP 서버] 클라이언트 연결 해제 : IP 주소 = " + array[0] + " 포트번호 = " + array[1]);
                        break;
                    }
                    else
                    {
                        listBox1.Items.Add("[TCP/" + array[0] + ":" + array[1] + "] " + strbyte);
                        ns.Write(recevbyte, 0, bytes);                          // Write() : client로 메시지 내용을 전송
                    }
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add("[TCP 서버] 클라이언트 연결 해제 : IP 주소 = " + array[0] + " 포트번호 = " + array[1]);
                    ex.ToString();
                    break;
                }
            }
            client.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            listener.Stop();
        }
    }
}

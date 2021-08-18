using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;                       // ArrayList를 사용하기 위해 선언
using System.Net.Sockets;                       // TcpClient를 사용하기 위해 선언
using System.Threading;                         // Thread를 사용하기 위해 선언
using System.Net;                               // IPAddress를 사용하기 위해 선언
using System.Runtime.Serialization.Formatters.Binary;   // BinaryFormatter를 사용하기 위해 선언

// 오류 : 그리라는 메세지를 보내는데 다른 창에서 메세지를 보냄, "Collection이 수정되었습니다." 오류 발생
// invalidate() 사용 안하면 정상 작동

namespace _20143184_하지훈
{
    public partial class Form1 : Form
    {
        ArrayList ar;                                                       // 배열 지정 데이터를 ArrayList에 저장
        TcpClient tclient;                          
        Thread th1;
       
        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th1 = new Thread(new ThreadStart(socketSendReceive));           // ThreadStart는 인자를 전달하지 않고 함수 등을 전달
            th1.IsBackground = true;                                        // true : Background 스레드(메인 스레드가 종료되면 바로 프로세스 종료), false : Foreground 스레드(메인 스레드가 종료되어도 Foreground 스레드가 살아있다면 프로세스가 종료되지 않고 계속 실행)
            th1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tclient = new TcpClient("127.0.0.1", 8000);                     // Server에서 그린 사각형을 Client로 넘겨줌

            if (tclient.Connected)                                          // 연결되었다면
                label1.Text = "연결 성공!";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Random Random = new Random();                                   // 랜덤 변수 지정

            if (e.Button == MouseButtons.Left)                              // 마우스 왼쪽 클릭 이벤트
            {
                CMyData c = new CMyData();                                  // CMyData 추가
                c.Size = (int)Random.Next(50, 100);                         // 사이즈 50~200 사이의 랜덤으로 지정
                c.Point = new Point(e.X, e.Y);                              // 마우스가 왼쪽에 클릭되는 위치 저장, 안 할 시 0,0 좌표에서 만들어짐
                ar.Add(c);                                                  // 배열에 저장

                if (tclient.Connected)                              // 연결되었다면
                {
                    NetworkStream ns = tclient.GetStream();         // Stream 차단 모드에서 소켓을 통해 데이터를 보내고 받는 메소드를 제공
                    BinaryFormatter bf = new BinaryFormatter();     // 객체를 이진 형식으로 직렬화 및 역직렬화하는 역할 
                    bf.Serialize(ns, c);                            // Serialize() : form을 대상으로 객체들을 직렬화하여 한 번에 보낼 수 있음, Serialize == Send, Write
                }
                else { label1.Text = "연결 끊김"; }

                //Invalidate();
                Graphics G = CreateGraphics();
                G.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, c.Size, c.Size);
                G.DrawEllipse(Pens.Black, c.Point.X, c.Point.Y, c.Size, c.Size);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in ar)                                       // ar안에 CMyData 추가
            {
                e.Graphics.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, c.Size, c.Size);
                e.Graphics.DrawEllipse(Pens.Black, c.Point.X, c.Point.Y, c.Size, c.Size);
            }
        }

        private void socketSendReceive()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 9000);    // TCP 네트워크 클라이언트에서 연결을 수신, Client에서 그린 사각형을 Server로 받음
            listener.Start();

            TcpClient serverClient = listener.AcceptTcpClient();    // 클라이언트 접속을 대기하고 있다가 접속 요청이 오면 이를 받아들여 TcpClient 객체를 생성하여 리턴

            if (serverClient.Connected)                             // 연결되었다면
            {
                NetworkStream ns = serverClient.GetStream();        // Stream 차단 모드에서 소켓을 통해 데이터를 보내고 받는 메소드를 제공
                //Encoding unicode = Encoding.Unicode;

                while (true)
                {
                    CMyData c = new CMyData();
                    BinaryFormatter bf = new BinaryFormatter();     // 객체를 이진 형식으로 직렬화 및 역직렬화하는 역할
                    c = (CMyData)bf.Deserialize(ns);                // Deserialize() : form을 대상으로 객체들을 역직렬화하여 한 번에 받을 수 있음, Deserialize == Recv, Read, File도 Deserialize로 바꿔봐라
                    ar.Add(c);

                    //Invalidate();
                    Graphics G = CreateGraphics();
                    G.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, c.Size, c.Size);
                    G.DrawEllipse(Pens.Black, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
            }
        }
    }

    [Serializable]
    public class CMyData
    {
        private Point point;
        private int size;

        public Point Point { get; set; }
        public int Size { get; set; }
    }
}

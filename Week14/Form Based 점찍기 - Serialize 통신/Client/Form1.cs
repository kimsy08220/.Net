using System;
using System.Collections.Generic;
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
        static Random random = new Random();
        private int R, G, B;

        private LinkedList<CMyData> total_lines;
        private CMyData c;
        private Color CurrentColor;                  // 현재 Color
        private Color pCurrentColor;
        private int iCurrentWidth = 2;               // 현재 Width
        private int iCurrentSize = 2;
        private int iCurrentShape = 0;

        [Serializable]
        class CMyData
        {
            private Color pcolor;
            private ArrayList Ar;
            public CMyData() { Ar = new ArrayList(); }
            public Color pColor
            {
                get
                {
                    return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                }
                set
                {
                    pcolor = value;
                }
            }
            public Color Color { get; set; }
            public Point Point { get; set; }
            public int Shape { get; set; }
            public int Width { get; set; }
            public int Size { get; set; }
        }


        ArrayList ar;                                               // 배열 지정 데이터를 ArrayList에 저장
        TcpClient tclient;                                          // TCP 네트워크 서비스에 대한 클라이언트 연결을 제공
        Thread th1;

        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            total_lines = new LinkedList<CMyData>();
            CurrentColor = Color.Black;               // Black로 초기화

            th1 = new Thread(new ThreadStart(socketSendReceive));   // ThreadStart는 인자를 전달하지 않고 함수 등을 전달
            th1.IsBackground = true;                                // true : Background 스레드(메인 스레드가 종료되면 바로 프로세스 종료), false : Foreground 스레드(메인 스레드가 종료되어도 Foreground 스레드가 살아있다면 프로세스가 종료되지 않고 계속 실행)
            th1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tclient = new TcpClient("127.0.0.1", 9000);             // Client에서 그린 사각형을 Server로 넘겨줌

            if (tclient.Connected)                                  // 연결되었다면
                label1.Text = "연결 성공!";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Random Random = new Random();                           // 랜덤 변수 지정

            if (e.Button == MouseButtons.Left)
            {
                c = new CMyData();
                c.Shape = iCurrentShape;
                c.Width = iCurrentWidth;
                c.Size = iCurrentSize;
                c.Point = new Point(e.X, e.Y);
                c.Color = CurrentColor;
                c.pColor = pCurrentColor;
                ar.Add(c.Point);

                if (tclient.Connected)                              // 연결되었다면
                {
                    NetworkStream ns = tclient.GetStream();         // Stream 차단 모드에서 소켓을 통해 데이터를 보내고 받는 메소드를 제공
                    BinaryFormatter bf = new BinaryFormatter();     // 객체를 이진 형식으로 직렬화 및 역직렬화하는 역할 
                    bf.Serialize(ns, c);                            // Serialize() : form을 대상으로 객체들을 직렬화하여 한 번에 보낼 수 있음, Serialize == Send, Write
                }
                else { label1.Text = "연결 끊김"; }

                Invalidate();

                //Graphics G = CreateGraphics();
                //G.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, c.Size, c.Size);
                //G.DrawEllipse(Pens.Black, c.Point.X, c.Point.Y, c.Size, c.Size);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in total_lines)
            {
                SolidBrush brc = new SolidBrush(c.Color); // brc  P.660
                Pen p = new Pen(c.pColor);
                if (c.Shape == 0)
                {
                    for (int i = 1; i < ar.Count; i++)
                    {
                        e.Graphics.DrawLine(new Pen(c.Color, c.Width), (Point)ar[i - 1], (Point)ar[i]);
                    }
                }
                else if (c.Shape == 1)
                {
                    e.Graphics.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    e.Graphics.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
                else if (c.Shape == 2)
                {
                    e.Graphics.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    e.Graphics.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
            }
        }

        private void socketSendReceive()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8000);    // TCP 네트워크 클라이언트에서 연결을 수신, Server에서 그린 사각형을 Client로 받음
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

                    Invalidate();
                    //Graphics G = CreateGraphics();
                    //G.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, c.Size, c.Size);
                    //G.DrawEllipse(Pens.Black, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
            }
        }

        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            dlg.DialogColor = CurrentColor;           // set
            dlg.iDialogWidth = iCurrentWidth;
            dlg.iDialogSize = iCurrentSize;

            dlg.shape = iCurrentShape;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CurrentColor = dlg.DialogColor;       // get 
                iCurrentWidth = dlg.iDialogWidth;
                iCurrentSize = dlg.iDialogSize;
                iCurrentShape = dlg.shape;
            }
            dlg.Dispose();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Capture && e.Button == MouseButtons.Left && c.Shape == 0)   // 모양이 직선일 때
            {
                Graphics G = CreateGraphics();
                G.DrawLine(new Pen(c.Color, c.Width), c.Point.X, c.Point.Y, e.X, e.Y);
                c.Point = new Point(e.X, e.Y);
                ar.Add(c.Point);
                G.Dispose();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(c);
            Invalidate();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 0;
            Invalidate();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 1;
            Invalidate();
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 2;
            Invalidate();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.FromArgb(255, 0, 0);
            Invalidate();
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.FromArgb(0, 255, 0);
            Invalidate();
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.FromArgb(0, 0, 255);
            Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 2;
            iCurrentSize = 2;
            Invalidate();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 4;
            iCurrentSize = 4;
            Invalidate();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 6;
            iCurrentSize = 6;
            Invalidate();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 8;
            iCurrentSize = 8;
            Invalidate();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 10;
            iCurrentSize = 10;
            Invalidate();
        }

        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            lineToolStripMenuItem.Checked = (iCurrentShape == 0);
            circleToolStripMenuItem.Checked = (iCurrentShape == 1);
            rectangleToolStripMenuItem.Checked = (iCurrentShape == 2);

            redToolStripMenuItem.Checked = (CurrentColor == Color.FromArgb(255, 0, 0));
            greenToolStripMenuItem.Checked = (CurrentColor == Color.FromArgb(0, 255, 0));
            blueToolStripMenuItem.Checked = (CurrentColor == Color.FromArgb(0, 0, 255));

            toolStripMenuItem2.Checked = (iCurrentWidth == 2);
            toolStripMenuItem3.Checked = (iCurrentWidth == 4);
            toolStripMenuItem4.Checked = (iCurrentWidth == 6);
            toolStripMenuItem5.Checked = (iCurrentWidth == 8);
            toolStripMenuItem6.Checked = (iCurrentWidth == 10);
        }
    }

    //[Serializable]      // [Serializable]를 통해 이 클래스는 메모리나 영구 저장 장치에 저장 가능
    //public class CMyData
    //{
    //    public Point Point { get; set; }
    //    public int Size { get; set; }
    //}
}

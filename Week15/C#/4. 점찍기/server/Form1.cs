using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace _1
{
    //server

    public partial class Form1 : Form
    {
        ArrayList ar;
        TcpClient tclient;
        Thread th1;
        
        private LinkedList<CMyData> total_lines;
        private CMyData c;
        private int iCurrentWidth = 2;
        private int iCurrentSize = 2;
        private int iCurrentshape = 0; // iCurrentshape = 0; iCurrentshape의 초기값을 0이라고 지정한다.
        private Color Currentcolor = Color.FromArgb(0, 0, 0);
        bool down = false;

        [Serializable]
        public class CMyData
        {
            private ArrayList Ar;
            public CMyData()
            {
                Ar = new ArrayList();
            }
            public ArrayList AR
            {
                get { return Ar; }
            }
            public Color pColor { get; set; }
            public Point Point { get; set; }
            public int Size { get; set; }
            public int Shape { get; set; }
            public int Width { get; set; }
        }

        public Form1()
        {
            ar = new ArrayList();
            total_lines = new LinkedList<CMyData>();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th1 = new Thread(new ThreadStart(socketSendReceive));
            th1.IsBackground = true;
            th1.Start();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                down = true;
                c = new CMyData();
                c.Shape = iCurrentshape;
                c.Width = iCurrentWidth;
                c.Size = iCurrentSize;
                c.Point = new Point(e.X, e.Y);
                c.pColor = Currentcolor;
                c.AR.Add(c.Point);

                if (tclient.Connected)
                {
                    NetworkStream ns = tclient.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ns, c);
                }
            }
            Invalidate();
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Capture && e.Button == MouseButtons.Left && c.Shape == 0)
            {
                Graphics G = CreateGraphics();
                G.DrawLine(new Pen(c.pColor, c.Width), c.Point.X, c.Point.Y, e.X, e.Y);
                c.Point = new Point(e.X, e.Y);
                c.AR.Add(c.Point);
                G.Dispose();

                if (tclient.Connected)
                {
                    NetworkStream ns = tclient.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ns, c);
                }
                               
            }
            Invalidate();
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(down)
            { 
            
            down = false;
            if (tclient.Connected)
            {
                NetworkStream ns = tclient.GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ns, c);
                total_lines.AddLast(c);
            }
             

            }
            Invalidate(); 
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in total_lines)
            {
                SolidBrush brc = new SolidBrush(c.pColor); // brc  P.660
                Pen p = new Pen(c.pColor);
                if (c.Shape == 0)
                {
                    for (int i = 1; i < c.AR.Count; i++)
                    {
                        e.Graphics.DrawLine(new Pen(c.pColor, c.Width), (Point)c.AR[i - 1], (Point)c.AR[i]);
                    }
                }
                else if (c.Shape == 1) // 
                {
                    e.Graphics.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size); // 
                    e.Graphics.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size); // 
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
            TcpListener listener = new TcpListener(IPAddress.Any, 9000);
            listener.Start();
            TcpClient serverClient = listener.AcceptTcpClient();
            if (serverClient.Connected)
            {
                NetworkStream ns = serverClient.GetStream();
                Encoding unicode = Encoding.Unicode;
                while (true)
                {
                    CMyData c = new CMyData();
                    BinaryFormatter bf = new BinaryFormatter();
                    c = (CMyData)bf.Deserialize(ns);

                    ar.Add(c);
                    //c.AR.Add(c);
                    //c.AR.Add(ar);
                    //Invalidate();
                    Graphics g = CreateGraphics();
                    SolidBrush brc = new SolidBrush(c.pColor); // brc  P.660
                    Pen p = new Pen(c.pColor);

                    if (c.Shape == 0)
                    {
                        for (int i = 1; i < c.AR.Count; i++)
                        {
                            g.DrawLine(new Pen(c.pColor, c.Width), (Point)c.AR[i - 1], (Point)c.AR[i]);
                        }
                    }
                    else if (c.Shape == 1) // 
                    {
                        g.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size); // 
                        g.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size); // 
                    }
                    else if (c.Shape == 2)
                    {
                        g.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                        g.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                    }

                }
            }
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            dlg.iDialogWidth = iCurrentWidth;
            dlg.shape = iCurrentshape;
            dlg.iDialogColor = Currentcolor;
            dlg.size = iCurrentSize;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                iCurrentSize = dlg.size;
                iCurrentWidth = dlg.iDialogWidth;
                iCurrentshape = dlg.shape;
                Currentcolor = dlg.iDialogColor;
            }
            dlg.Dispose();
        }
        private void 동그라미ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentshape = 1;
            Invalidate();
        }
        private void 사각형ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentshape = 2;
            Invalidate();
        }
        private void 직선ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentshape = 0;
            Invalidate();
        }
        private void 빨강ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Currentcolor = Color.FromArgb(255, 0, 0);
        }
        private void 초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Currentcolor = Color.FromArgb(0, 255, 0);
        }
        private void 파랑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Currentcolor = Color.FromArgb(0, 0, 255);
        }
        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            직선ToolStripMenuItem.Checked = (iCurrentshape == 0);
            동그라미ToolStripMenuItem.Checked = (iCurrentshape == 1);
            사각형ToolStripMenuItem.Checked = (iCurrentshape == 2);
            빨강ToolStripMenuItem.Checked = (Currentcolor == Color.FromArgb(255, 0, 0));
            초록ToolStripMenuItem.Checked = (Currentcolor == Color.FromArgb(0,255,0));
            파랑ToolStripMenuItem.Checked = (Currentcolor == Color.FromArgb(0, 0, 255));
            toolStripMenuItem2.Checked = (iCurrentWidth == 2);
            toolStripMenuItem3.Checked = (iCurrentWidth == 4);
            toolStripMenuItem4.Checked = (iCurrentWidth == 6);
            toolStripMenuItem5.Checked = (iCurrentWidth == 8);
            toolStripMenuItem6.Checked = (iCurrentWidth == 10);
            toolStripMenuItem2.Checked = (iCurrentSize == 2);
            toolStripMenuItem3.Checked = (iCurrentSize == 4);
            toolStripMenuItem4.Checked = (iCurrentSize == 6);
            toolStripMenuItem5.Checked = (iCurrentSize == 8);
            toolStripMenuItem6.Checked = (iCurrentSize == 10);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 2;
            iCurrentSize = 2;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 4;
            iCurrentSize = 4;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 6;
            iCurrentSize = 6;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 8;
            iCurrentSize = 8;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            iCurrentWidth = 10;
            iCurrentSize = 10;
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\temp";
            saveFileDialog1.Title = "파일 저장하기";
            saveFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();         // 도움을 받음
                bf.Serialize(fs, total_lines);                      // 파일을 바이트로 변환하여 저장
                fs.Close();
            }
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\temp";
            openFileDialog1.Title = "파일 불러오기";
            openFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter bf = new BinaryFormatter();
                    total_lines = (LinkedList<CMyData>)bf.Deserialize(fs);      // 디버그 실행 시 더블 클릭 했을 때 선 3개를 저장했는데 total_lines에는 4개를 저장 때문에 예외 발생
                    fs.Close();
                    Invalidate();
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("지정한 파일이 없습니다.");
                }
            }
            //Currentcolor = Color.FromArgb(0, 0, 0); // 불러오기 할때 색을 검정으로 바꾸기
            //iCurrentSize = 2; // 불러오기 할때 모양중 도형 크기를 2로 바꾸기
            //iCurrentWidth = 2;  // 불러오기 할때 모양중 선의 두께를 2로 바꾸기
            //iCurrentshape = 0; // 불러오기 할때 모양을 선으로 바꾸기
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tclient = new TcpClient("127.0.0.1", 8000);
            if (tclient.Connected)
            {
                label1.Text = "연결 성공!";
            }
        }

        
    }
}

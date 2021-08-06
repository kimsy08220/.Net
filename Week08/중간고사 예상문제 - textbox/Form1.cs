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
using System.IO;                                        // Stream 사용하기 위해 선언
using System.Runtime.Serialization.Formatters.Binary;   // BinaryFormatter 사용하기 위해 선언

namespace 중간고사_예상문제
{
    public partial class Form1 : Form
    {
        private LinkedList<CMyData> total_lines;
        private CMyData c;
        private Color CurrentColor;                  // 현재 Color
        private int iCurrentWidth = 2;               // 현재 Width
        private int iCurrentSize = 2;
        private int iCurrentShape = 0;
        bool down = false;                              // 이걸 넣음으로써 더블 클릭 예외 해결

        [Serializable]
        class CMyData
        {
            private ArrayList Ar;
            public CMyData() { Ar = new ArrayList(); }
            public Color Color { get; set; }
            public Point Point { get; set; }
            public int Shape { get; set; }
            public int Width { get; set; }
            public int Size { get; set; }
            public ArrayList AR { get { return Ar; } }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            total_lines = new LinkedList<CMyData>(); 
            CurrentColor = Color.Black;               // Black로 초기화
            //iCurrentWidth = 1;                      // Width 1로 초기화
            //iCurrentShape = 0;                      // Shape 0(Line)로 초기화
            //iCurrentShape = 2;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                down = true;                        // 이걸 넣음으로써 더블 클릭 예외 해결
                c = new CMyData();
                c.Shape = iCurrentShape; 
                c.Width = iCurrentWidth;
                c.Size = iCurrentSize;
                c.Point = new Point(e.X, e.Y); 
                c.Color = CurrentColor; 
                c.AR.Add(c.Point);
            }
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)       // 직선을 그릴 때만 MouseMove 사용
        {
            if (Capture && e.Button == MouseButtons.Left && c.Shape == 0)   // 모양이 직선일 때
            {
                Graphics G = CreateGraphics();
                G.DrawLine(new Pen(c.Color, c.Width), c.Point.X, c.Point.Y, e.X, e.Y);
                c.Point = new Point(e.X, e.Y);
                c.AR.Add(c.Point);
                G.Dispose();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)         // 직선을 그릴 때만 MouseUp 사용
        {
            if (down)                            // 이걸 넣음으로써 더블 클릭 예외 해결
            {
                total_lines.AddLast(c);
                Invalidate();
                down = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in total_lines)
            {
                SolidBrush brc = new SolidBrush(c.Color); // brc  P.660
                Pen p = new Pen(c.Color);
                if (c.Shape == 0)
                {
                    for (int i = 1; i < c.AR.Count; i++)
                    {
                        e.Graphics.DrawLine(new Pen(c.Color, c.Width), (Point)c.AR[i - 1], (Point)c.AR[i]);
                    }
                }
                else if (c.Shape == 1) 
                {
                    e.Graphics.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size , c.Size); 
                    e.Graphics.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
                else if (c.Shape == 2)
                {
                    e.Graphics.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    e.Graphics.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
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

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 0;
            Invalidate();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 1;
            Invalidate();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void menuStrip1_MenuActivate(object sender, EventArgs e)            // Click이 아니라 Activate
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

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void 불러오ToolStripMenuItem_Click(object sender, EventArgs e)
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

            // 불러오기 후 초기화 작업
            CurrentColor = Color.FromArgb(0, 0, 0);
            iCurrentWidth = 2;
            iCurrentSize = 2;
            iCurrentShape = 0;
        }

    }
}

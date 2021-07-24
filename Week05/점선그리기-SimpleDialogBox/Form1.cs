using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace mixControl
{
    public partial class Form1 : Form
    {
        private LinkedList<CMyData> total_lines;
        private CMyData mydata;
        private Point p;

        private int iCurrentShape;
        private Color CurrentColor;
        private int iCurrentWidth = 1;

        public Form1()
        {
            CurrentColor = Color.FromArgb(255, 0, 0);
            total_lines = new LinkedList<CMyData>();
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mydata = new CMyData();
                mydata.Shape = iCurrentShape;

                if (iCurrentShape == 2)
                    mydata.Width = iCurrentWidth;
                else
                    mydata.Width = 10*iCurrentWidth;

                mydata.Color = CurrentColor;
                p = new Point(e.X, e.Y);
                mydata.AR.Add(p);

                SolidBrush b = new SolidBrush(CurrentColor);
                Graphics g = this.CreateGraphics();
                if (iCurrentShape == 0)
                {
                   g.FillRectangle(b, e.X,e.Y,10*iCurrentWidth,10*iCurrentWidth);
                   g.DrawRectangle(new Pen(Color.Black), e.X, e.Y, 10 * iCurrentWidth, 10 * iCurrentWidth);
                }
                if (iCurrentShape == 1)
                {
                    g.FillEllipse(b, e.X, e.Y, 10 * iCurrentWidth, 10 * iCurrentWidth);
                    g.DrawEllipse(new Pen(Color.Black), e.X, e.Y, 10 * iCurrentWidth, 10 * iCurrentWidth);
                }

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Capture && e.Button == MouseButtons.Left && iCurrentShape == 2)
            {
                Graphics G = CreateGraphics();
                G.DrawLine(new Pen(mydata.Color, mydata.Width), p.X, p.Y, e.X, e.Y);
                p = new Point(e.X, e.Y);
                mydata.AR.Add(p);
                G.Dispose();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(CurrentColor);
         
            foreach (CMyData line in total_lines)
            {
                b = new SolidBrush(line.Color);
                 if (line.Shape == 0)
                 {
                        e.Graphics.FillRectangle(b, ((Point)line.AR[0]).X, ((Point)line.AR[0]).Y, line.Width, line.Width);
                        e.Graphics.DrawRectangle(new Pen(Color.Black), ((Point)line.AR[0]).X, ((Point)line.AR[0]).Y, line.Width, line.Width);
                 }
                 else if (line.Shape == 1)
                 {
                     e.Graphics.FillEllipse(b, ((Point)line.AR[0]).X, ((Point)line.AR[0]).Y, line.Width, line.Width);
                     e.Graphics.DrawEllipse(new Pen(Color.Black), ((Point)line.AR[0]).X, ((Point)line.AR[0]).Y, line.Width, line.Width);
                 }
                 else
                 {
                     for (int i = 1; i < line.AR.Count; i++)
                         e.Graphics.DrawLine(new Pen(line.Color, line.Width), (Point)line.AR[i - 1], (Point)line.AR[i]);
                 }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(mydata);
            mydata = new CMyData();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          //  CurrentColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
        }

        private void 원형ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 0;    
            Invalidate();
        }

        private void 사각형ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 1;  
            Invalidate();
        }

        private void 자유곡선ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 2;       
            Invalidate();
        }


        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            원형ToolStripMenuItem.Checked = (iCurrentShape ==0);
            사각형ToolStripMenuItem.Checked = (iCurrentShape == 1);
            자유곡선ToolStripMenuItem.Checked = (iCurrentShape == 2);

        }

        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg=new Form2();
            dlg.Shape = iCurrentShape;
            //dlg.BackColor = DialogPenColor;
            if (dlg.ShowDialog() == DialogResult.OK)
                iCurrentShape = dlg.Shape;    
            dlg.Dispose();
        }
    }

          #region Def Class
        public class CMyData
        {
            private Color color;//펜컬러
            private int width;//크기
            private int shape;
            private ArrayList Ar;

            public CMyData()  //생성자함수
            {
                Ar = new ArrayList();
            }
            public Color Color//펜컬러
            {
                get { return color; }
                set { color = value; }
            }
            public int Width//크기
            {
                get { return width; }
                set { width = value; }
            }
            public int Shape
            {
                get { return shape; }
                set { shape = value; }
            }
            public ArrayList AR
            {
                get { return Ar; }        
            }
        }
        #endregion
	

}

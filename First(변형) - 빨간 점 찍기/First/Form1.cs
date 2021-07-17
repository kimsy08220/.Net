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

namespace First
{
    public partial class Form1 : Form   // :는 상속의 의미, Form(닷넷에 있는 클래스 라이브러리)이라는 클래스를 상속
    {
        ArrayList ar;
        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)  
        {
            Random Random = new Random();

            if (e.Button == MouseButtons.Left)
            {
                CMyData c = new CMyData();
                c.Shape = (int)Random.Next(2);
                c.Size = (int)Random.Next(50, 200);
                c.Point = new Point(e.X, e.Y);
                c.bColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));   
                c.pColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                ar.Add(c);
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in ar)   
            {
                SolidBrush brc = new SolidBrush(c.bColor);
                Pen p = new Pen(c.pColor);
                if (c.Shape == 1) 
                {
                    e.Graphics.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    e.Graphics.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
                else
                {
                    e.Graphics.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    e.Graphics.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
            }
        }

        public class CMyData
        {
            private Point point;
            private Color penCol;
            private Color brushCol;
            private int size, shape;

            public Color pColor
            {
                get { return penCol; }  
                set { penCol = value; } 
            }
            public Color bColor
            {
                get { return brushCol; }     
                set { brushCol = value; }
            }
            public Point Point
            {
                get { return point; }
                set { point = value; }
            }
            public int Size
            {
                get { return size; }
                set { size = value; }
            }
            public int Shape
            {
                get { return shape; }
                set { shape = value; }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class Form1 : Form
    {
        public ArrayList ar;
        public Random Random;
        public CMyData c;

        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
            Random = new Random();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == (MouseButtons)1048576)
            {
                c = new CMyData();
                c.Shape = (int)Random.Next(2);
                c.Size = (int)Random.Next(50, 200);
                c.Point = new Point(e.X, e.Y);
                c.bColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                c.pColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                ar.Add(c);
            }

            //Invalidate();  

            Graphics G = CreateGraphics();
            SolidBrush brc = new SolidBrush(c.bColor);
            Pen p = new Pen(c.pColor);

            if (c.Shape == 1)
            {
                G.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                G.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
            }
            else
            {
                G.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                G.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics G = e.Graphics;    // 604p, Graphics 객체 빼냄, Graphics G = CreateGraphics();와 동일 

            foreach (CMyData c in ar)
            {
                SolidBrush brc = new SolidBrush(c.bColor);
                Pen p = new Pen(c.pColor);

                if (c.Shape == 1)
                {
                    G.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);
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
            public Point Point { get; set; }
            public Color pColor { get; set; }
            public Color bColor { get; set; }
            public int Size { get; set; }
            public int Shape { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

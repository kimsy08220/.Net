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
        public CMyData c;

        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics G = CreateGraphics();

            if (e.Button == (MouseButtons)1048576)
            {
                c = new CMyData();
                c.Point = new Point(e.X, e.Y);
                ar.Add(c);
                G.DrawEllipse(Pens.Red, e.X, e.Y, 6, 6);
                G.FillEllipse(Brushes.Red, e.X, e.Y, 6, 6);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics G = e.Graphics;    // 604p, Graphics 객체 빼냄, Graphics G = CreateGraphics();와 동일 

            foreach (CMyData c in ar)
            {
                G.DrawEllipse(Pens.Red, c.Point.X, c.Point.Y, 6, 6);
                G.FillEllipse(Brushes.Red, c.Point.X, c.Point.Y, 6, 6);
            }
        }

        public class CMyData
        {
            public Point Point { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;               // ArrayList 사용하기 위해 선언

namespace _2021_계절학기_닷넷_중간고사
{
    public partial class Form1 : Form
    {
        private Color CurrentColor;
        private int CurrentShape;
        private ArrayList ar;
        CMyData data;
        Bitmap B;

        class CMyData
        {
            public Point Point { get; set; }
            public Color Color { get; set; }
            public int Shape { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
            CurrentColor = Color.Black;
            CurrentShape = 0;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                data = new CMyData();
                data.Point = new Point(e.X, e.Y);
                data.Color = CurrentColor;
                data.Shape = CurrentShape;
                ar.Add(data);
            }
            Invalidate();

            //Graphics G = CreateGraphics();
            //Pen p = new Pen(data.Color);
            //SolidBrush brc = new SolidBrush(data.Color);

            //if (data.Shape == 0)
            //{
            //    G.DrawRectangle(p, data.Point.X, data.Point.Y, 10, 10);
            //    G.FillRectangle(brc, data.Point.X, data.Point.Y, 10, 10);
            //}
            //else if (data.Shape == 1)
            //{
            //    G.DrawEllipse(p, data.Point.X, data.Point.Y, 10, 10);
            //    G.FillEllipse(brc, data.Point.X, data.Point.Y, 10, 10);
            //}
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (B == null || B.Width != ClientSize.Width || B.Height != ClientSize.Height)
                B = new Bitmap(ClientSize.Width, ClientSize.Height);
            
            Graphics G = Graphics.FromImage(B);
            G.Clear(SystemColors.Window);

            foreach (CMyData c in ar)
            {
                Pen p = new Pen(c.Color);
                SolidBrush brc = new SolidBrush(c.Color);

                if (c.Shape == 0)
                {
                    G.DrawRectangle(p, c.Point.X, c.Point.Y, 10, 10);
                    G.FillRectangle(brc, c.Point.X, c.Point.Y, 10, 10);
                }
                else if (c.Shape == 1)
                {
                    G.DrawEllipse(p, c.Point.X, c.Point.Y, 10, 10);
                    G.FillEllipse(brc, c.Point.X, c.Point.Y, 10, 10);
                }
            }

            if (B != null)
                e.Graphics.DrawImage(B, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)     // Bitmap
        {
            //base.OnPaintBackground(e);
        }

        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            dlg.DialogColor = CurrentColor;
            dlg.DialogShape = CurrentShape;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CurrentColor = dlg.DialogColor;
                CurrentShape = dlg.DialogShape;
            }
            dlg.Dispose();
        }
    }
}

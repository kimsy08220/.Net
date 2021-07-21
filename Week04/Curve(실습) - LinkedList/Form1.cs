using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;       // ArrayList를 사용하기 위해 선언

namespace Curve
{
	public partial class Form1 : Form
	{
        private LinkedList<ArrayList> total_lines;
        private ArrayList ar; 
		private int x, y;

		public Form1()
		{
			InitializeComponent();
            total_lines = new LinkedList<ArrayList>();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                ar = new ArrayList();
				x = e.X;
				y = e.Y;
                ar.Add(new Point(x, y));
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (Capture && e.Button == MouseButtons.Left)       // 왼쪽 버튼을 누른 상태로 움직일 때
			{
				Graphics G = CreateGraphics();
				G.DrawLine(Pens.Black, x, y, e.X, e.Y);
				x = e.X;
				y = e.Y;
                ar.Add(new Point(x, y));
				G.Dispose();
			}
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (ArrayList line in total_lines)
            {
                for (int i = 1; i < line.Count; i++)
                    e.Graphics.DrawLine(Pens.Black, (Point)line[i - 1], (Point)line[i]); 
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(ar);         // ar 배열에 있는 정보를 total_lines에 추가
        }
	}
}

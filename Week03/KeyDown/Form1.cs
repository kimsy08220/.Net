using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyDown
{
	public partial class Form1 : Form
	{
		private int x, y;

		public Form1()
		{
			InitializeComponent();
			x = 10;
			y = 10;
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillEllipse(Brushes.Black, x - 8, y - 8, 16, 16);    // (x - 8, y - 8) 좌표, width :16, height : 16인 원
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys)37:
					x -= 10;
					//Invalidate();
					break;
				case Keys.Right:
					x += 10;
					//Invalidate();
					break;
				case Keys.Up:
					y -= 10;
					//Invalidate();
					break;
				case Keys.Down:
					y += 10;
					//Invalidate();
					break;
			}
            Invalidate();
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
	}
}

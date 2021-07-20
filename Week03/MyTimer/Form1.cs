using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyTimer
{
	public partial class Form1 : Form
	{
		private string Time;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Timer T = new Timer();
			T.Interval = 1000;
			T.Tick += new EventHandler(Form1_Timer);
			T.Start();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawString(Time, this.Font, Brushes.Black, 10, 10);
		}

		private void Form1_Timer(object sender, System.EventArgs e)
		{
			DateTime dt = DateTime.Now;
			Time = dt.Hour + "시 " + dt.Minute + "분 " + dt.Second + "초";
			Invalidate();
		}
	}
}

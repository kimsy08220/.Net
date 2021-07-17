using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TypingOneway
{
	public partial class Form1 : Form
	{
		private string str, all_str, correct_str;
		private char current_char;
		private int x, y, all_int, correct_int;

		Random random = new Random();

		public Form1()
		{
			InitializeComponent();
			
			all_int = 0;
			correct_int = 0;

			str = "";     // 출제 문자
			all_str = "";
			correct_str = "";

            next_char();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Timer T = new Timer();
			T.Interval = 500;
			T.Tick += new EventHandler(Form1_Timer);
			T.Start();
		}
		private void Form1_Timer(object sender, System.EventArgs e)     // 시간이 지날 때마다 호출?
		{
            y += 100;

            if (y > ClientRectangle.Height)     // y좌표가 화면 밖을 벗어날 때
                next_char();
            else
            {
                Invalidate();
                /*
                Rectangle rc = new Rectangle(x, y - 100, 20, 20);
                Invalidate(rc);
                Graphics G = CreateGraphics();
                G.DrawString(str, Font, Brushes.Black, x, y);
                //*/
            }
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			str = "" + current_char;
			all_str = "모든 문자 수 : " + all_int;
			correct_str = "맞춘 문자 수 : " + correct_int;

			e.Graphics.DrawString(str, Font, Brushes.Black, x, y);
			e.Graphics.DrawString(all_str, Font, Brushes.Black, 10, 10);
			e.Graphics.DrawString(correct_str, Font, Brushes.Black, 10, 30);
		}

		private void next_char()    // 모든 문자 수를 증가하고 새로운 알파벳이 나오게 하는 역할
		{
			all_int += 1;
            current_char = Convert.ToChar(random.Next('a', 'z' + 1));
            x = random.Next(0, ClientRectangle.Width);
			y = 10;
            Invalidate();
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == current_char)
				correct_int += 1;
			next_char();
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyPress
{
	public partial class Form1 : Form
	{
		private string str;

		public Form1()
		{
			InitializeComponent();
			str = "";
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
            e.Graphics.DrawString(str, this.Font, Brushes.Black, 10, 10);    // DrawString : 문자열 출력, this.Font == Font
		}

        // 문자를 누를 때마다 Form1_KeyPress 발생
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)     // KeyPressEventArgs : 사용자가 누른 문자를 넘겨받음
		{
            if (e.KeyChar == ' ')   // space bar를 누를 때
                str = "";
            else if (e.KeyChar == (char)Keys.Back)              // e.KeyChar == '\b'
            {             
                if (str.Length > 0)
                    str = str.Substring(0, str.Length - 1);     // str = str.Remove(str.Length - 1, 1); // str.Length - 1 인덱스부터 1개 remove
            }
            else
                str += e.KeyChar;
			Invalidate();
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
	}
}

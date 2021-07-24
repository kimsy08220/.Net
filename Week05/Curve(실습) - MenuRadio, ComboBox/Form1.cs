using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Curve
{
	public partial class Form1 : Form
	{
        private LinkedList<CMyData> total_lines;
        private CMyData data;
        private Color iCurrentPenColor;             // 현재 Color
        private int iCurrentPenWidth;               // 현재 Width
		private int x, y;

		public Form1()
		{
			InitializeComponent();
            total_lines = new LinkedList<CMyData>();
            iCurrentPenColor = Color.Red;           // Red로 초기화
          
            for (int i = 2; i <= 10; i+=2)
            {
                comboBox1.Items.Add(i);             // Convert.ToString(i) == i, 항목 추가
                comboBox1.SelectedIndex = iCurrentPenWidth / 2 - 1;     // index에 추가
            }
            iCurrentPenWidth = 2;                   // iCurrentPenWidth를 2로 초기화
            comboBox1.SelectedIndex = 0;            // comboBox1의 index를 0으로 초기화
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				x = e.X;
				y = e.Y;
                data = new CMyData();
                data.AR.Add(new Point(x, y));
                data.Color = iCurrentPenColor;      // CMyData 타입의 Color에 현재 Color를 저장
                data.Width = iCurrentPenWidth;      // CMyData 타입의 Width에 현재 Width 저장
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (Capture && e.Button == MouseButtons.Left)       // 왼쪽 버튼을 누른 상태로 움직일 때
			{
				Graphics G = CreateGraphics();
                Pen p = new Pen(data.Color, data.Width);        // 현재 Color, Width Pen 객체 생성
				G.DrawLine(p, x, y, e.X, e.Y);
				x = e.X;
				y = e.Y;
                data.AR.Add(new Point(x, y));
				G.Dispose();
			}
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData line in total_lines)
            {
                Pen p = new Pen(line.Color, line.Width);        // 현재 Color, Width Pen 객체 생성
                for (int i = 1; i < line.AR.Count; i++)
                    e.Graphics.DrawLine(p, (Point)line.AR[i - 1], (Point)line.AR[i]); 
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(data);     
        }

        class CMyData
        {
            private ArrayList Ar;

            public CMyData()  { Ar = new ArrayList(); }
            public Color Color { get; set; }
            public int Width { get; set; }
            public ArrayList AR { get { return Ar; } }
        }

        private void 빨강ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Red;
            radioButton1.Checked = true;
        }

        private void 초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Green;
            radioButton2.Checked = true;
        }

        private void 파랑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Blue;
            radioButton3.Checked = true;
        }

        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            빨강ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Red);
            초록ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Green);
            파랑ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Blue);
        }

        private void ColorRadioButton_CheckedChanged(object sender, EventArgs e)    // radioButton1,2,3을 하나로 통합, radiobutton을 object 타입의 sender로 업 캐스팅(안정)해서 들고옴
        {
            RadioButton R = (RadioButton)sender;        // 781p, RadioButton R = sender as RadioButton;와 동일, sender : radiobutton을 가르킴(다운 캐스팅 : 불안정)
            
            if (R == radioButton1)
                iCurrentPenColor = Color.Red;
            if (R == radioButton2)
                iCurrentPenColor = Color.Green;
            if (R == radioButton3)
                iCurrentPenColor = Color.Blue;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)     // 선택 기능
        {
            iCurrentPenWidth = (comboBox1.SelectedIndex + 1) * 2;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)              // 선택 기능이 아닌 직접 입력
        {
            iCurrentPenWidth = int.Parse(comboBox1.Text);
        }
	}
}

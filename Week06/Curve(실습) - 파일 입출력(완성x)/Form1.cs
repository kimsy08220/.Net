using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;                                        // Stream 사용하기 위해 선언
using System.Runtime.Serialization.Formatters.Binary;   // BinaryFormatter 사용하기 위해 선언

namespace Curve
{
	public partial class Form1 : Form
	{
        private LinkedList<CMyData> total_lines;
        private CMyData data;
        private Color CurrentPenColor;             // 현재 Color
        private int iCurrentPenWidth;
		private int x, y;

		public Form1()
		{
			InitializeComponent();
            total_lines = new LinkedList<CMyData>();
            CurrentPenColor = Color.Black;           // Black로 초기화
            iCurrentPenWidth = 2;
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				x = e.X;
				y = e.Y;
                data = new CMyData();
                data.AR.Add(new Point(x, y));
                data.Color = CurrentPenColor;      // CMyData 타입의 Color에 현재 Color 색인 Red를 저장
                data.Width = iCurrentPenWidth;
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (Capture && e.Button == MouseButtons.Left)       // 왼쪽 버튼을 누른 상태로 움직일 때
			{
				Graphics G = CreateGraphics();
                Pen p = new Pen(data.Color, data.Width);                    // Red Pen 객체 생성
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
                Pen p = new Pen(line.Color, line.Width);
                for (int i = 1; i < line.AR.Count; i++)
                    e.Graphics.DrawLine(p, (Point)line.AR[i - 1], (Point)line.AR[i]); 
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(data);       // ar 배열에 있는 정보를 total_lines에 추가
        }

        [Serializable]
        class CMyData
        {
            private ArrayList Ar;

            public CMyData()  { Ar = new ArrayList(); }
            public Color Color { get; set; }
            public int Width { get; set; }
            public ArrayList AR { get { return Ar; } }
        }

        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            dlg.DialogPenColor = CurrentPenColor;
            dlg.iDialogPenWidth = iCurrentPenWidth;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CurrentPenColor = dlg.DialogPenColor;     
                iCurrentPenWidth = dlg.iDialogPenWidth;
            }
            dlg.Dispose();
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\temp";
            saveFileDialog1.Title = "파일 저장하기";
            saveFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();         // 도움을 받음
                bf.Serialize(fs, total_lines);                      // 파일을 바이트로 변환하여 저장
                fs.Close();
            }
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\temp";
            openFileDialog1.Title = "파일 불러오기";
            openFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                total_lines = (LinkedList<CMyData>)bf.Deserialize(fs);               
                fs.Close();
            }
        }

	}
}

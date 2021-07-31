using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 더블 버퍼링을 하기 전, 껌벅 해결 : DoubleBuffered를 true로 하면 껌벅x
namespace DoubleBuffer
{
    public partial class Form1 : Form
    {
        int ex = 10, ey = 100;
        const int r = 15;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int x, y;

            // 가로, 세로 줄 긋기
            for (x = 0; x < ClientRectangle.Right; x += 10)
                e.Graphics.DrawLine(Pens.Black, x, 0, x, ClientRectangle.Bottom);
            for (y = 0; y < ClientRectangle.Bottom; y += 10)
                e.Graphics.DrawLine(Pens.Black, 0, y, ClientRectangle.Right, y);

            // 원 그리기
            e.Graphics.FillEllipse(new SolidBrush(Color.LightGreen), ex - r, ey - r, r * 2, r * 2);
            e.Graphics.DrawEllipse(new Pen(Color.Blue, 5), ex - r, ey - r, r * 2, r * 2);
        }

        private void timer1_Tick(object sender, EventArgs e)        // 도구상자 timer를 가져오고 속성창을 클릭
        {
            ex += 6;
            if (ex > ClientRectangle.Right) 
                ex = 0;
            Invalidate();
        }
    }
}


// 더블 버퍼링을 하고 난 후
//namespace DoubleBuffer
//{
//    public partial class Form1 : Form
//    {
//        int ex = 10, ey = 100;
//        const int r = 15;
//        Bitmap B;               // 여분 도화지 역할(선이 그어져있는 도화지)

//        public Form1()
//        {
//            InitializeComponent();
//            timer1.Start();
//        }

//        private void Form1_Paint(object sender, PaintEventArgs e)
//        {
//            if (B != null)
//                e.Graphics.DrawImage(B, 0, 0);
//        }

//        private void timer1_Tick(object sender, EventArgs e)          // 도구상자 timer를 가져오고 속성창을 클릭
//        {
//            int x, y;

//            if (B == null || B.Width != ClientSize.Width || B.Height != ClientSize.Height)
//                B = new Bitmap(ClientSize.Width, ClientSize.Height);

//            Graphics G = Graphics.FromImage(B);
//            G.Clear(SystemColors.Window);           // 흔적 지우기

//            ex += 6;
//            if (ex > ClientRectangle.Right) 
//                ex = 0;

//            // 가로, 세로 줄 긋기
//            for (x = 0; x < ClientRectangle.Right; x += 10)
//                G.DrawLine(Pens.Black, x, 0, x, ClientRectangle.Bottom);
//            for (y = 0; y < ClientRectangle.Bottom; y += 10)
//                G.DrawLine(Pens.Black, 0, y, ClientRectangle.Right, y);

//            // 원 그리기
//            G.FillEllipse(new SolidBrush(Color.LightGreen), ex - r, ey - r, r * 2, r * 2);
//            G.DrawEllipse(new Pen(Color.Blue, 5), ex - r, ey - r, r * 2, r * 2);

//            Invalidate();       // Paint로 이동
//        }
//    }
//}

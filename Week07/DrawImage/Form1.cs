using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawImage
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			Image I = Image.FromFile("아기.jpg");
			e.Graphics.DrawImage(I, 0, 0);                  // (표시 이미지, 시작 x좌표, 시작 y좌표)
			//e.Graphics.DrawImage(I, new Point(0, 0));     // 위 코드와 동일

            //e.Graphics.DrawImage(I, new Rectangle(0, 0, 200, 169)); // (표시 이미지, (시작 x좌표, 시작 y좌표, width, height))
            //e.Graphics.DrawImage(I, 0, 0, 200, 169);      // 위 코드와 동일

            //e.Graphics.DrawImage(I, 10, 10, new Rectangle(70, 20, 300, 320), GraphicsUnit.Pixel);     // (표시 이미지, 시작 x좌표, 시작 y좌표, 원래 사진(시작 x좌표, 시작 y좌표, width, height)에서 자른 사각형)
            //e.Graphics.DrawImage(I, new Rectangle(10,10,200,200), 70, 20, 300, 300, GraphicsUnit.Pixel);  

            // 사각형을 pts(지정 좌표)로 출력
            //Rectangle R = new Rectangle(70, 20, 300, 320);
            //Point[] pts = { new Point(0, 0), new Point(300, 0), new Point(100, 200) };
            //e.Graphics.DrawImage(I, pts, R, GraphicsUnit.Pixel);
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
	}
}

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
            Bitmap B = new Bitmap(I);                   // Bitmap : pixel로 이루어짐

            for (int y = 0; y < B.Height; y++)
                for (int x = 0; x < B.Width; x++)
                {
                    Color pixel = B.GetPixel(x, y);     // RGB pixel을 가져옴
                     
                    byte r = (byte)pixel.R;             // ~(byte)pixel.R : 반전
                    byte g = (byte)pixel.G;
                    byte b = (byte)pixel.B;

                    //if (r > 255)                      // pixel은 8bit인데 오버플로우나서 까맣게 변하는 현상을 방지
                    //    r = 255;
                    //if (g > 255)
                    //    g = 255;
                    //if (b > 255)
                    //    b = 255;

                    B.SetPixel(x, y, Color.FromArgb(r, g, b));

                    //int k = (r + g + b) / 3;
                    //B.SetPixel(x, y, Color.FromArgb(k, k, k));
                }
            e.Graphics.DrawImage(B, 0, 0);
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
	}
}

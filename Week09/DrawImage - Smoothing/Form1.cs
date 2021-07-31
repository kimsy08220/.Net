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
            Image I = Image.FromFile("lenna_noise.jpg");
            Bitmap B = new Bitmap(I);

            // 흑백으로 바꾸는 작업
            for (int y = 0; y < B.Height; y++)
            {
                for (int x = 0; x < B.Width; x++)
                {
                    Color color = B.GetPixel(x, y);
                    byte r = (byte)((color.R + color.G + color.B) / 3);
                    B.SetPixel(x, y, Color.FromArgb(r, r, r));
                }
            }

            // smoothing mask 작업 : 사진을 부드럽게(노이즈를 완화) 해주는 작업
            // 마스크 : 마스크 크기만큼의 주변 값의 영향을 받도록 하여 마스크 중심 위치의 픽셀 값을 조정하고 이를 영상 전체에 대해 적용하도록 하는 회선 기법
	        Bitmap Smoothing = new Bitmap(B);       
            int[,] m = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };		
            int sum;

            for (int x = 1; x < B.Width - 1; x++)
            {
                for (int y = 1; y < B.Height - 1; y++)
                {
                    sum = 0;
                    for (int r = -1; r < 2; r++)
                    {
                        for (int c = -1; c < 2; c++)
                            sum += m[r + 1, c + 1] * B.GetPixel(x + r, y + c).R; 	// 마스크 씌우는 작업
                     }
	                sum = sum / 9;
                    Smoothing.SetPixel(x, y, Color.FromArgb(sum, sum, sum)); 
                }
            }
            e.Graphics.DrawImage(B, 0, 0);
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

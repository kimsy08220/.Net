using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;                                        // Stream 사용하기 위해 선언
using System.Runtime.Serialization.Formatters.Binary;   // BinaryFormatter 사용하기 위해 선언


namespace DrawImage
{
    public partial class Form1 : Form
    {
        Image I;
        Bitmap B;
        public Form1()      // Form1 속성 : IsMdiContainer = True
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 파일열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image I = Image.FromFile(openFileDialog1.FileName);

                Form2 Child = new Form2();
                Child.image = I;                // 이미지가 Form2으로 넘어옴
                Child.MdiParent = this;
                Child.Show();
            }
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)        // saveFileDialog1 속성 : Filter = jpg파일|*.jpg|bmp파일|*.bmp|png파일|*.png
        {
            saveFileDialog1.FileName = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Form2 Child = (Form2)this.ActiveMdiChild;       // Form2 Child = ActiveMdiChild as Form2;와 동일, ActiveMdiChild : 선택된 창
                if (Child != null)
                {
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            Child.image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case 2:
                            Child.image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 3:
                            Child.image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                }
            }
        }

        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Child = (Form2)this.ActiveMdiChild;           // Form2 Child = ActiveMdiChild as Form2;와 동일, ActiveMdiChild : 선택된 창
            if (Child != null)
                Child.Close();

            //Form2 form = ActiveMdiChild as Form2;
            //form.Dispose();
        }

        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)       // 사진을 부드럽게
        {
            Form2 original = (Form2)this.ActiveMdiChild;

            if (original != null)
            {
                // 흑백으로 바꿔주는 역할
                Bitmap gBitmap = new Bitmap(original.image);
                //https://docs.microsoft.com/ko-kr/dotnet/api/system.drawing.imaging.pixelformat?view=netframework-4.8
                if (gBitmap.PixelFormat.ToString() != "Format8bppIndexed")          // Format8bppIndexed = 인덱싱된, 픽셀당 8비트 형식으로 지정 따라서 색상표에 256 색이 포함, 즉 흑백이 아닐 때
                {
                    for (int i = 0; i < gBitmap.Height; i++)
                    {
                        for (int j = 0; j < gBitmap.Width; j++)
                        {
                            int color = gBitmap.GetPixel(j, i).R + gBitmap.GetPixel(j, i).G + gBitmap.GetPixel(j, i).B;
                            color /= 3;
                            Color c = Color.FromArgb(color, color, color);
                            gBitmap.SetPixel(j, i, c);
                        }
                    }
                }

                // Smoothing Mask
                Bitmap Smoothing = new Bitmap(original.image);
                int[,] m = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                int sum;

                for (int x = 1; x < gBitmap.Width - 1; x++)
                {
                    for (int y = 1; y < gBitmap.Height - 1; y++)
                    {
                        sum = 0;
                        for (int r = -1; r < 2; r++)
                        {
                            for (int c = -1; c < 2; c++)
                                sum += m[r + 1, c + 1] * gBitmap.GetPixel(x + r, y + c).R;
                        }
                        sum /= 9;
                        Smoothing.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
                    }
                }

                Form2 child = new Form2();
                child.image = Smoothing;
                child.MdiParent = this;
                child.Show();
            }
        }   

        //     0    245   0    => 차이
        // 255 | 255 | 10 | 10 => 차이 0은 검은색, 245는 하얀색으로 표시
        private void edgeToolStripMenuItem_Click(object sender, EventArgs e)        // 경계선. 색깔이 변하는 곳에서 흰색으로, 나머진 검은색
        {
            Form2 original = ActiveMdiChild as Form2;

            if (original != null)
            {
                // 흑백으로 바꿔주는 역할
                Bitmap gBitmap = new Bitmap(original.image);
                //https://docs.microsoft.com/ko-kr/dotnet/api/system.drawing.imaging.pixelformat?view=netframework-4.8
                if (gBitmap.PixelFormat.ToString() != "Format8bppIndexed")          // Format8bppIndexed = 흑백이 아니면 흑백으로 바꿔줌
                {
                    for (int i = 0; i < gBitmap.Height; i++)
                    {
                        for (int j = 0; j < gBitmap.Width; j++)
                        {
                            int color = gBitmap.GetPixel(j, i).R + gBitmap.GetPixel(j, i).G + gBitmap.GetPixel(j, i).B;
                            color /= 3;
                            Color c = Color.FromArgb(color, color, color);
                            gBitmap.SetPixel(j, i, c);
                        }
                    }
                }

                // Edge
                Bitmap Edge = new Bitmap(gBitmap);
                int[,] m = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                int sum;
                for (int x = 1; x < gBitmap.Width - 1; x++)
                {
                    for (int y = 1; y < gBitmap.Height - 1; y++)
                    {
                        sum = 0;
                        for (int r = -1; r < 2; r++)
                        {
                            for (int c = -1; c < 2; c++)
                                sum += m[r + 1, c + 1] * gBitmap.GetPixel(x + r, y + c).R;
                        }
                        sum = Math.Abs(sum);                    // 0에서 255의 차이는 -255이기 때문에 마이너스 값이 나오지 않게 하기 위해 절댓값을 취해줌
                        if (sum > 255) sum = 255;
                        if (sum < 0) sum = 0;
                        Edge.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
                    }
                }

                Form2 child = new Form2();
                child.image = Edge;
                child.MdiParent = this;
                child.Show();
            }
        }
    }
}

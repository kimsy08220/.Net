using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MDI_IMAGE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image I = Image.FromFile(openFileDialog1.FileName);
                Form2 Child = new Form2();
                Child.image = I;
                Child.MdiParent = this;
                Child.Show();
            }
        }

        private void 밝게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Child = ActiveMdiChild as Form2;
            if (Child != null)
            {
                Image I = Child.image;
                Bitmap Bit = new Bitmap(I);
                for (int i = 0; i < Bit.Height; i++)
                {
                    for (int j = 0; j < Bit.Width; j++)
                    {
                        Color color = Bit.GetPixel(j, i);
                        int r = color.R + 50; if (r > 255) r = 255;
                        int g = color.G + 50; if (g > 255) g = 255;
                        int b = color.B + 50; if (b > 255) b = 255;
                        Bit.SetPixel(j, i, Color.FromArgb(r, g, b));
                    }
                }
                Form2 Child2 = new Form2();
                Child2.image = Bit;
                Child2.MdiParent = this;
                Child2.Show();
            }
        }

        private void 어둡게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Child = ActiveMdiChild as Form2;
            if (Child != null)
            {
                Image I = Child.image;
                Bitmap Bit = new Bitmap(I);
                for (int i = 0; i < Bit.Height; i++)
                {
                    for (int j = 0; j < Bit.Width; j++)
                    {
                        Color color = Bit.GetPixel(j, i);
                        int r = color.R - 50; if (r < 0) r = 0;
                        int g = color.G - 50; if (g < 0) g = 0;
                        int b = color.B - 50; if (b < 0) b = 0;
                        Bit.SetPixel(j, i, Color.FromArgb(r, g, b));
                    }
                }
                Form2 Child2 = new Form2();
                Child2.image = Bit;
                Child2.MdiParent = this;
                Child2.Show();
            }
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Form2 Child = ActiveMdiChild as Form2;
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
            Form2 form = ActiveMdiChild as Form2;
            form.Dispose();
        }

        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 original = ActiveMdiChild as Form2;
            if (original != null)
            {
                Bitmap gBitmap = new Bitmap(original.image);
                //https://docs.microsoft.com/ko-kr/dotnet/api/system.drawing.imaging.pixelformat?view=netframework-4.8
                if (gBitmap.PixelFormat.ToString() != "Format8bppIndexed")
                {
                    for (int i = 0; i < gBitmap.Height; i++)
                    {
                        for (int j = 0; j < gBitmap.Width; j++)
                        {
                            int color = gBitmap.GetPixel(j,i).R +  gBitmap.GetPixel(j, i).G + gBitmap.GetPixel(j, i).B;
                            color /= 3;
                            Color c = Color.FromArgb(color,color,color);
                            gBitmap.SetPixel(j, i, c);
                        }
                    }
                    
                }
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
                            {
                                sum += m[r + 1, c + 1] * gBitmap.GetPixel(x + r, y + c).R;
                            }
                        }
                        sum = Math.Abs(sum);
                        sum /= 9;
                        if (sum > 255) sum = 255;
                        if (sum < 0) sum = 0;
                        Smoothing.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
                    }
                }
                Form2 child = new Form2();
                child.image = Smoothing;
                child.MdiParent = this;
                child.Show();
            }
        }

        private void edgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 original = ActiveMdiChild as Form2;
            
            if (original != null)
            {
                Bitmap gBitmap = new Bitmap(original.image);
                //https://docs.microsoft.com/ko-kr/dotnet/api/system.drawing.imaging.pixelformat?view=netframework-4.8
                if (gBitmap.PixelFormat.ToString() != "Format8bppIndexed")
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
                            {

                                sum += m[r + 1, c + 1] * gBitmap.GetPixel(x + r, y + c).R;

                            }
                        }
                        sum = Math.Abs(sum);
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

        private void medianToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 original = ActiveMdiChild as Form2;
            if (original != null)
            {
                Bitmap gBitmap = new Bitmap(original.image);
                //https://docs.microsoft.com/ko-kr/dotnet/api/system.drawing.imaging.pixelformat?view=netframework-4.8
                if (gBitmap.PixelFormat.ToString() != "Format8bppIndexed")
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
                Bitmap Median = new Bitmap(gBitmap);
                int[] m = new int[9];
                for (int x = 1; x < gBitmap.Width - 1; x++)
                {
                    for (int y = 1; y < gBitmap.Height - 1; y++)
                    {
                        for (int r = -1; r < 2; r++)
                        {
                            for (int c = -1; c < 2; c++)
                                m[(r + 1) * 3 + (c + 1)] = gBitmap.GetPixel(x + r, y + c).R;
                        }
                        Array.Sort(m);
                        Median.SetPixel(x, y, Color.FromArgb(m[4], m[4], m[4]));
                    }
                }
                Form2 child = new Form2();
                child.image = Median;
                child.MdiParent = this;
                child.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

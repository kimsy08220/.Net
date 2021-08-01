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
    public partial class Form2 : Form
    {
        public Image image { get; set;  }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e) 
        {
            this.ClientSize = new Size(image.Width, image.Height);
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);       // 이미지 크기에 맞춰서 나옴
        }
    }
}

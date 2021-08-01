using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDI_IMAGE
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public Image image
        {
            get;
            set;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
           // e.Graphics.DrawImage(image, 0, 0)
           // 최초불러올때 그림크기가 작다.크기를 추가지정해보았다.p.704 된다....
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(image.Width, image.Height);
        }
    }
}

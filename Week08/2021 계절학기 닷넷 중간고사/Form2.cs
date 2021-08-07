using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2021_계절학기_닷넷_중간고사
{
    public partial class Form2 : Form
    {
        public Color DialogColor { get; set; }
        public int DialogShape;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("사각형");
            comboBox1.Items.Add("타원");

            if (DialogShape == 0) 
                comboBox1.Text = "사각형";
            if (DialogShape == 1) 
                comboBox1.Text = "타원";

            hScrollBar1.Value = DialogColor.R;
            hScrollBar2.Value = DialogColor.G;
            hScrollBar3.Value = DialogColor.B;

            textBox1.Text = DialogColor.R.ToString();
            textBox2.Text = DialogColor.G.ToString();
            textBox3.Text = DialogColor.B.ToString();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            textBox1.Text = hScrollBar1.Value.ToString();
            textBox2.Text = hScrollBar2.Value.ToString();
            textBox3.Text = hScrollBar3.Value.ToString();
            label5.Invalidate();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            DialogShape = comboBox1.SelectedIndex;
            if (DialogShape == 0) 
                comboBox1.Text = "사각형";
            if (DialogShape == 1) 
                comboBox1.Text = "타원";
            label5.Invalidate();
        }

        private void label5_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(DialogColor);
            SolidBrush brc = new SolidBrush(DialogColor);

            if (DialogShape == 0)
            {
                e.Graphics.DrawRectangle(p, label5.Width / 2 - 25, label5.Height / 2 - 25, 50, 50);
                e.Graphics.FillRectangle(brc, label5.Width / 2 - 25, label5.Height / 2 - 25, 50, 50);
            }
            else if (DialogShape == 1)
            {
                e.Graphics.DrawEllipse(p, label5.Width / 2 - 25, label5.Height / 2 - 25, 50, 50);
                e.Graphics.FillEllipse(brc, label5.Width / 2 - 25, label5.Height / 2 - 25, 50, 50);
            }
        }
    }
}
